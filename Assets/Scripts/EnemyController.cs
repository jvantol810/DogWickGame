using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Gun;
    public GameObject Player;
    public Rigidbody2D rigidbody2d;
    public EnemyShooting enemyShooting;
    float health;
    public float maxHealth;
    public float speed;
    public float moveCooldownTime;
    private bool trackPlayer;
    public float shootDistance;
    public float shootTime;
    private float shootTimer;
    private bool shooting;
    public bool facingLeft;
    public Vector2 gunOffset;
    private Vector2 positionRelative;
    public Animator animator;
    private bool turnSpriteRed;
    public float spriteRedTime;
    private float spriteRedTimer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        enemyShooting = GetComponent<EnemyShooting>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        shootTimer = shootTime;
        Player = GameObject.Find("Player");
        trackPlayer = true;
        shooting = false;
        spriteRedTimer = spriteRedTime;
    }

    // Update is called once per frame
    void Update()
    {
        positionRelative = transform.InverseTransformPoint(Player.GetComponent<Transform>().position);   
        if(positionRelative.x < 0f)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        if (shooting)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
        }
        if (turnSpriteRed)
        {
            if(spriteRedTimer < 0)
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
        Debug.Log("Idle: " + shooting);
        animator.SetBool("FacingLeft", facingLeft);
        //If the character is facing right, flip gun to be right.
        if (facingLeft)
        {
            Gun.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            Gun.GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Aim();
    }
    private void Move()
    {
        //Check if the enemy can move towards player--if he's in a move zone, don't move him here
        if (trackPlayer && !shooting)
        {
            
            if (Vector2.Distance(Player.GetComponent<Rigidbody2D>().position, rigidbody2d.position) < shootDistance)
           {
               
                shooting = true;
            }
            else
            {
                //Move towards the player with a degree of innacuracy
                Debug.Log("MOVE TOWARDS PLAYER");
                Vector3 target = new Vector3(Player.GetComponent<Rigidbody2D>().position.x, Player.GetComponent<Rigidbody2D>().position.y);
                Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, speed * Time.deltaTime);
                rigidbody2d.MovePosition(newPosition);
            }
        }
        else if(trackPlayer && shooting)
        {
            Debug.Log("STOP AND SHOOT!");
            if (shootTimer > 0)
            {
               enemyShooting.Shoot();
                
                
                shootTimer -= Time.deltaTime;
            }
            else
            {
                shootTimer = shootTime;
                shooting = false;
            }
            
        }


    }
    private void Aim()
    {
        Transform gunTransform = Gun.transform;
        Transform playerTransform = Player.transform;
        //gunTransform.LookAt(Player.transform.position);
        Vector3 gunPosition2D = gunTransform.position;
        Vector3 aimDirection = (playerTransform.position - gunPosition2D).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        gunTransform.eulerAngles = new Vector3(0, 0, angle);

        if (facingLeft)
        {
            gunTransform.position = new Vector3(transform.position.x - gunOffset.x, transform.position.y + gunOffset.y, transform.position.z);
        }
        else
        {
            gunTransform.position = new Vector3(transform.position.x + gunOffset.x, transform.position.y + gunOffset.y, transform.position.z);
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

        if(collision.tag == "PlayerBullet")
        {
            //Handle knockback
            Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
            rigidbody2d.AddForce(5f * -dir, ForceMode2D.Impulse);
            //Call hit function
            Hit(1);
            Destroy(collision.gameObject);
        }
    }

    //Check if the enemy is in a move zone
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WallEdge")
        {
            trackPlayer = false;
            //Calculate distance 2to bottom and top of wall, find whichever one is closer
            Vector3 wallDimensions = new Vector3(collision.transform.localScale.x * collision.GetComponent<BoxCollider2D>().size.x,
                collision.transform.localScale.y * collision.GetComponent<BoxCollider2D>().size.y, 0);
            float wallTop = collision.transform.position.y + wallDimensions.y / 2;
            float wallBottom = collision.transform.position.y - wallDimensions.y / 2;
            float wallLeft = collision.transform.position.x - wallDimensions.x / 2;
            float wallRight = collision.transform.position.x + wallDimensions.x / 2;
            float distanceToTop = Vector2.Distance(new Vector2(wallDimensions.x, wallTop), collision.transform.position);
            float distanceToBottom = Vector2.Distance(new Vector2(wallDimensions.y, wallBottom), collision.transform.position);
            float distanceToRight = Vector2.Distance(new Vector2(wallRight, wallDimensions.y), collision.transform.position);
            float distanceToLeft = Vector2.Distance(new Vector2(wallLeft, wallDimensions.y), collision.transform.position);
            //If the wall is vertical, find the shortest distance (either top or bottom) and navigate to it
            if (collision.gameObject.GetComponent<WallEdgeController>().vertical)
            {
                if (distanceToTop > distanceToBottom)
                {
                    Vector3 target = new Vector3(rigidbody2d.position.x, rigidbody2d.position.y + speed * Time.deltaTime, 0);
                    Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
                    rigidbody2d.MovePosition(newPosition);
                }
                else
                {
                    Vector3 target = new Vector3(rigidbody2d.position.x, rigidbody2d.position.y - speed * Time.deltaTime, 0);
                    Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
                    rigidbody2d.MovePosition(newPosition);
                }
            }


            //Otherwise, the wall is horziontal--find the shortest distance (either left or right) and navigate to it
            else
            {
                Debug.Log("Horizontal wall edge");
                if (distanceToRight < distanceToLeft)
                {
                    Vector3 target = new Vector3(rigidbody2d.position.x - speed * Time.deltaTime, rigidbody2d.position.y, 0);
                    Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
                    rigidbody2d.MovePosition(newPosition);
                }
                else
                {
                    Vector3 target = new Vector3(rigidbody2d.position.x + speed * Time.deltaTime, rigidbody2d.position.y, 0);
                    Vector3 newPosition = Vector3.MoveTowards(rigidbody2d.position, target, 0.1f);
                    rigidbody2d.MovePosition(newPosition);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WallEdge")
        {
            trackPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            
        }
    }
}
