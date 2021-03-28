using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody2D rigidbody2d;
    public RoomManager roomManager;
    float health;
    public float maxHealth;
    public float speed;
    public float moveInnacuracyMin;
    public float moveInnacuracyMax;
    private Vector2 positionRelative;
    public bool facingLeft;
    public Animator animator;
    private bool turnSpriteRed;
    public float spriteRedTime;
    private float spriteRedTimer;
    private bool trackPlayer;
    private bool movingAwayFromBarrel;
    public float movingAwayFromBarrelTime;
    private float movingAwayFromBarrelTimer;
    private GameObject Barrel;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = maxHealth;
        Player = GameObject.Find("Player");
        animator = gameObject.GetComponent<Animator>();
        spriteRedTimer = spriteRedTime;
        trackPlayer = true;
        movingAwayFromBarrelTimer = movingAwayFromBarrelTime;
    }

    // Update is called once per frame
    void Update()
    {
        positionRelative = transform.InverseTransformPoint(Player.GetComponent<Transform>().position);
        if (positionRelative.x < 0f)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        animator.SetBool("FacingLeft", facingLeft);
        if (turnSpriteRed)
        {
            if (spriteRedTimer < 0)
            {
                turnSpriteRed = false;
            }
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            spriteRedTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            spriteRedTimer = spriteRedTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (trackPlayer)
        {
            float step = speed * Time.fixedDeltaTime;
            //Check if the melee enemy is close to the player--if he is, move towards the player with total accuracy
            float distance = Vector3.Distance(rigidbody2d.position, Player.GetComponent<Rigidbody2D>().position);
            //Move towards the player with a degree of innacuracy
            float horizontalMoveInnacuracy = Random.Range(moveInnacuracyMin, moveInnacuracyMax);
            float verticalMoveInnacuracy = Random.Range(moveInnacuracyMin, moveInnacuracyMax);
            Vector3 target = new Vector3(Player.GetComponent<Rigidbody2D>().position.x, Player.GetComponent<Rigidbody2D>().position.y, 0);
            Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, step);
            rigidbody2d.MovePosition(newPosition);
        }
        else if (movingAwayFromBarrel)
        {
            if(Barrel == null)
            {
                trackPlayer = true;
                movingAwayFromBarrel = false;
            }
            else if(movingAwayFromBarrelTimer > 0)
            {
                positionRelative = transform.InverseTransformPoint(Barrel.GetComponent<Transform>().position);
                Vector3 target = new Vector3(rigidbody2d.position.x,rigidbody2d.position.y,0);
                Vector3 newPosition;
                if (positionRelative.x < 0f)
                {
                    //MoveLeft
                    Debug.Log("MoveLeft");
                    target.x = rigidbody2d.position.x + speed * Time.deltaTime;
                }
                else
                {
                    //MoveRight
                    Debug.Log("MoveRight");
                    target.x = rigidbody2d.position.x - speed * Time.deltaTime;
                }
                if(positionRelative.y < 0f)
                {
                    //MoveDown
                    Debug.Log("MoveDown");
                    target.y = rigidbody2d.position.y + speed * Time.deltaTime;
                }
                else
                {
                    //MoveUp
                    Debug.Log("MoveUp");
                    target.y = rigidbody2d.position.y - speed * Time.deltaTime;
                }
                Debug.Log("TARGET: " + target.ToString());
                newPosition = Vector3.MoveTowards(rigidbody2d.position, target, speed * Time.fixedDeltaTime);
                rigidbody2d.MovePosition(newPosition);
                movingAwayFromBarrelTimer -= Time.deltaTime;
            }
            else
            {
                movingAwayFromBarrelTimer = movingAwayFromBarrelTime;
                trackPlayer = true;
                movingAwayFromBarrel = false;
                
            }
            
        }
        
        
    }


    void Hit(float damage)
    {
        //If the enemy is hit, reduce its healt by damage
        //Also check if its health is 0 -- if it is, destroy it
        health -= damage;
        turnSpriteRed = true;
        if (health <= 0)
        {
            Player.GetComponent<PlayerController>().kills++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "PlayerBullet")
        {
            //Call hit function
            Hit(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.tag == "WallEdge")
        {
            trackPlayer = false;
            Barrel = collision.gameObject;
            movingAwayFromBarrel = true;
        }
        //if (collision.tag == "WallEdge")
        //{
        //    trackPlayer = false;
        //    //Calculate distance 2to bottom and top of wall, find whichever one is closer
        //    Vector3 wallDimensions = new Vector3(collision.transform.localScale.x * collision.GetComponent<BoxCollider2D>().size.x,
        //        collision.transform.localScale.y * collision.GetComponent<BoxCollider2D>().size.y, 0);
        //    float wallTop = collision.transform.position.y + wallDimensions.y / 2;
        //    float wallBottom = collision.transform.position.y - wallDimensions.y / 2;
        //    float wallLeft = collision.transform.position.x - wallDimensions.x / 2;
        //    float wallRight = collision.transform.position.x + wallDimensions.x / 2;
        //    float distanceToTop = Vector2.Distance(new Vector2(wallDimensions.x, wallTop), collision.transform.position);
        //    float distanceToBottom = Vector2.Distance(new Vector2(wallDimensions.y, wallBottom), collision.transform.position);
        //    float distanceToRight = Vector2.Distance(new Vector2(wallRight, wallDimensions.y), collision.transform.position);
        //    float distanceToLeft = Vector2.Distance(new Vector2(wallLeft, wallDimensions.y), collision.transform.position);
        //    //If the wall is vertical, find the shortest distance (either top or bottom) and navigate to it
        //    if (collision.gameObject.GetComponent<WallEdgeController>().vertical)
        //    {
        //        if (distanceToTop > distanceToBottom)
        //        {
        //            Vector3 target = new Vector3(rigidbody2d.position.x, rigidbody2d.position.y + speed * Time.deltaTime, 0);
        //            Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
        //            rigidbody2d.MovePosition(newPosition);
        //        }
        //        else
        //        {
        //            Vector3 target = new Vector3(rigidbody2d.position.x, rigidbody2d.position.y - speed * Time.deltaTime, 0);
        //            Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
        //            rigidbody2d.MovePosition(newPosition);
        //        }
        //    }


        //    //Otherwise, the wall is horziontal--find the shortest distance (either left or right) and navigate to it
        //    else
        //    {
        //        Debug.Log("Horizontal wall edge");
        //        if (distanceToRight < distanceToLeft)
        //        {
        //            Vector3 target = new Vector3(rigidbody2d.position.x - speed * Time.deltaTime, rigidbody2d.position.y, 0);
        //            Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
        //            rigidbody2d.MovePosition(newPosition);
        //        }
        //        else
        //        {
        //            Vector3 target = new Vector3(rigidbody2d.position.x + speed * Time.deltaTime, rigidbody2d.position.y, 0);
        //            Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
        //            rigidbody2d.MovePosition(newPosition);
        //        }
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WallEdge")
        {
            trackPlayer = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            trackPlayer = false;
            Barrel = collision.gameObject;
            movingAwayFromBarrel = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            
        }
    }

}
