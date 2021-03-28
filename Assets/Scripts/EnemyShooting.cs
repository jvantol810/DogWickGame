using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public GameObject Player;
    public RoomManager roomManager;
    public Transform gunTransform;
    public GameObject Gun;
    public Transform gunEndPointTransform;
    public EnemyController enemyController;
    public float numberOfShots;
    public float bulletForce;
    public float shootCooldownTime;
    private float shootCooldownTimer;
    public float shootCooldownTimeMin;
    public float shootCooldownTimeMax;
    public bool ricochet;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        shootCooldownTimer = shootCooldownTime;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        //Decrement the cooldown timer
        shootCooldownTimer -= Time.deltaTime;
        //Check if the enemy can shoot
        if (shootCooldownTimer < 0)
        {
            //Fire a bullet towards the position of the player
            Vector3 playerPosition = Player.transform.position;
            GameObject bullet = Instantiate(enemyBulletPrefab, gunTransform.position, gunTransform.rotation);
            if (ricochet)
            {
                bullet.GetComponent<BulletController>().ricochet = true;
                Debug.Log("Ricochet: " + bullet.GetComponent<BulletController>().ricochet);
            }
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

            //apply force to bullet depending on which way enemy is facing

            if (enemyController.facingLeft)
            {
                bulletRigidbody.AddForce(gunEndPointTransform.right * bulletForce, ForceMode2D.Impulse);
                Gun.GetComponent<Animator>().Play("gunFireLeft", 0);
            }
            else
            {
                bulletRigidbody.AddForce(gunEndPointTransform.right * bulletForce, ForceMode2D.Impulse);
                Gun.GetComponent<Animator>().Play("gunFireRight", 0);
            }

            //Reset the cooldown timer
            shootCooldownTimer = shootCooldownTime;
        }
    }

   
}
