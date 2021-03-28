using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    public float gunOffsetX;
    public float gunOffsetY;
    public int maxHealth = 5;
    public float timeInvincible = 6;
    public float kills;

    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    Transform transform;
    public GameObject gun;
    public Camera camera;
    private GameObject[] cameraObjects;
    public GameObject loyaltyUIObject;
    float horizontal;
    float vertical;

    Animator animator;
    public LoyaltyUIController loyaltyUI;
    Vector2 lookDirection = new Vector2(1, 0);
    Vector2 mousePosition;



    AudioSource audioSource;
    public AudioClip damageClip;
    public ParticleSystem hitEffect;
    public ParticleSystem healthCollectEffect;
    private bool facingRight;
    bool gunIsAdjusted;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        GameObject.DontDestroyOnLoad(camera);
        GameObject.DontDestroyOnLoad(loyaltyUIObject);
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        transform = GetComponent<Transform>();
        currentHealth = maxHealth;
        invincibleTimer = timeInvincible;
        kills = 0;
        gunIsAdjusted = false;
        facingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 move = new Vector2(horizontal, vertical);

        //Set animator x and y values to the x and y values based on key input
        animator.SetFloat("MoveX", move.x);
        animator.SetFloat("MoveY", move.y);

        if(horizontal + vertical == 0)
        {
            //animator.SetBool("Idle", true);
            if (facingRight)
            {
                animator.Play("DogWickIdleRight");
            }
            else
            {
                animator.Play("DogWickIdleLeft");
            }
            
        }

        //moving right
        else if (horizontal == 1) {
            facingRight = true;
            animator.Play("DogWickMoveRight");
            
        }
        //moving left
        else if (horizontal == -1)
        {
            facingRight = false;
            animator.Play("DogWickMoveLeft");
        }
        //moving up/down and facing left or right
        else if(vertical == 1 && horizontal == 0 || vertical == -1 && horizontal == 0)
        {
            //face whatever direction you were previously
            if (facingRight)
            {
                animator.Play("DogWickMoveRight");
            }
            else
            {
                animator.Play("DogWickMoveLeft");
            }
        }
        //If the character is facing right, flip gun to be right.
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("DogWickIdleRight") || animator.GetCurrentAnimatorStateInfo(0).IsName("DogWickMoveRight"))
        //{
        //    gun.GetComponent<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    gun.GetComponent<SpriteRenderer>().flipX = false;
        //}

        //animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        cameraObjects = GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameraObjects.Length > 1)
        {
            GameObject.Destroy(cameraObjects[0]);
        }

    }

    void FixedUpdate()
    {
        Move();
        Aim();
    }

    private void Move()
    {
        Vector2 position = rigidbody2d.position;
        Vector2 move = new Vector2(horizontal, vertical);

        position += move * speed * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(position);
    }
    private void Aim()
    {
        Transform gunTransform = gun.transform;
        gunTransform.LookAt(mousePosition);
        Vector2 gunPosition2D = gunTransform.position;
        Vector3 aimDirection = (mousePosition - gunPosition2D).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        gunTransform.eulerAngles = new Vector3(0, 0, angle);

        //Vector3 localScale = new Vector3(0.05f, 0.05f, 0);
        //if (angle > 90 || angle < -90)
        //{
        //    localScale.y = -0.05f;
        //}
        //else
        //{
        //    localScale.y = 0.05f;
        //}
        //gunTransform.localScale = localScale;

        if (facingRight)
        {
            gunTransform.position = new Vector3(transform.position.x - gunOffsetX, transform.position.y + gunOffsetY, transform.position.z);
        }
        else
        {
            gunTransform.position = new Vector3(transform.position.x + gunOffsetX * 2f, transform.position.y + gunOffsetY, transform.position.z);
        }

    }

    void Hit(int damage)
    {
        //If the enemy is hit, reduce its healt by damage
        //Also check if its health is 0 -- if it is, destroy it
        if (!isInvincible)
        {
            currentHealth -= damage;
            loyaltyUI.removeLoyaltyIcon();
 
            invincibleTimer = timeInvincible;
            isInvincible = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
            Hit(1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Hit(1);
        }
        if(collision.gameObject.name == "MeleeEnemy")
        {
            //Handle knockback
            Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
            rigidbody2d.AddForce(40f * -dir, ForceMode2D.Impulse);
        }
    }

    public void healToFull()
    {
        currentHealth = maxHealth;
    }

    public void addMaxLoyalty()
    {
        maxHealth++;
    }
}
