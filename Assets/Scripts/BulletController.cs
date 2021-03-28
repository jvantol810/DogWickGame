using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletLifetime;
    public bool ricochet;
    float bulletLifeTimer;
    public LayerMask collisionMask;
    private void Awake()
    {
        bulletLifeTimer = bulletLifetime;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bulletLifeTimer -= Time.deltaTime;
        if (bulletLifeTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
    }
}
