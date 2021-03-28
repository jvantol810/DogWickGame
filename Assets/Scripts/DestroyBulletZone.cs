using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletZone : MonoBehaviour
{
    public LayerMask collisionMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet" || collision.tag == "PlayerBullet")
        {
            if (collision.GetComponent<BulletController>().ricochet)
            {
                Rigidbody2D bulletRb = collision.GetComponent<Rigidbody2D>();
                Vector3 ricochetForce = new Vector3(bulletRb.position.x * -5f, bulletRb.position.y * -5f);
                bulletRb.AddForce(ricochetForce, ForceMode2D.Impulse);
            }
            else
            {
                Destroy(collision.gameObject);
            }
            
        }
    }
}
