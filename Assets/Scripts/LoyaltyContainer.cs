using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyaltyContainer : MonoBehaviour
{
    public GameObject Player;
    float playerLoyalty;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        Player = GameObject.Find("Player");
        playerLoyalty = Player.GetComponent<PlayerController>().health;
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player");
        playerLoyalty = Player.GetComponent<PlayerController>().health;
        Debug.Log("player health is " + playerLoyalty);
    }
}
