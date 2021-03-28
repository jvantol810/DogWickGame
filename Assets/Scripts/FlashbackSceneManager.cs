using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlashbackSceneManager : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject Player;
    public GameObject LoyaltyUI;
    public GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Camera = GameObject.Find("Main Camera");
        PlayerController player = Player.GetComponent<PlayerController>();
        LoyaltyUI = GameObject.Find("LoyaltyLabel");
        GameObject.Destroy(Player);
        GameObject.Destroy(LoyaltyUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
           
            GameObject.Destroy(Camera);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
