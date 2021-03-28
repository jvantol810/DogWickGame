using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunEnemyShooting : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public GameObject Player;
    public RoomManager roomManager;
    public Transform gunTransform;
    public Transform gunEndPointTransform;

    public float bulletForce;
    public float shootCooldownTime;
    private float shootCooldownTimer;
    public float shootCooldownTimeMin;
    public float shootCooldownTimeMax;
    // Start is called before the first frame update
    void Start()
    {
        shootCooldownTimer = shootCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        //Decrement the cooldown timer
        shootCooldownTimer -= Time.deltaTime;
        //Check if the enemy can shoot
        if (shootCooldownTimer < 0)
        {
            //Fire a bullet towards the position of the player
            Vector3 playerPosition = Player.transform.position;
            GameObject bullet = Instantiate(enemyBulletPrefab, gunTransform.position, gunTransform.rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.AddForce(gunTransform.right * bulletForce, ForceMode2D.Impulse);
            Debug.DrawLine(gunEndPointTransform.position, playerPosition);

            //Reset the cooldown timer
            shootCooldownTimer = shootCooldownTime;
        }
    }

}
