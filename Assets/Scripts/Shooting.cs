using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunEndPointTransform;
    public Transform gunTransform;
    public GameObject Gun;
    private float gunCooldownTimer;
    public float gunCooldownTime;
    public float bulletForce = 20f;
    public Animator animator;
    Vector2 mousePosition;
    public AudioClip shootingClip;
    public AudioSource audioSource;
    public Camera camera;
    private void Start()
    {
        gunCooldownTimer = gunCooldownTime;
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") && gunCooldownTimer < 0)
        {
            Shoot();
            gunCooldownTimer = gunCooldownTime;
        }
        gunCooldownTimer -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunEndPointTransform.position, gunEndPointTransform.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        bulletRigidbody.AddForce(gunEndPointTransform.right * bulletForce, ForceMode2D.Impulse);
        Gun.GetComponent<Animator>().Play("gunFireRight", 0);

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("DogWickIdleLeft") || animator.GetCurrentAnimatorStateInfo(0).IsName("DogWickMoveLeft"))
        //{
        //    bulletRigidbody.AddForce(gunEndPointTransform.right * -bulletForce, ForceMode2D.Impulse);
        //    Gun.GetComponent<Animator>().Play("gunFireLeft", 0);
        //}
        //else
        //{
        //    bulletRigidbody.AddForce(gunEndPointTransform.right * bulletForce, ForceMode2D.Impulse);
        //    Gun.GetComponent<Animator>().Play("gunFireRight", 0);
        //}
        

    }
}
