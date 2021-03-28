using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScreenManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject LoyaltyUI;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        GameObject.Destroy(Player);
        LoyaltyUI = GameObject.Find("LoyaltyLabel");
        GameObject.Destroy(LoyaltyUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("Room1");
        }
    }
}
