using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    public GameObject Player;
    public GameObject LoyaltyUI;
    // Start is called before the first frame update
    void Start()
    {
        
    Player = GameObject.Find("Player");
        Destroy(Player);
        LoyaltyUI = GameObject.Find("LoyaltyLabel");
        Destroy(LoyaltyUI);
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
