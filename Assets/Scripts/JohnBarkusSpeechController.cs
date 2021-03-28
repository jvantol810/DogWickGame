using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnBarkusSpeechController : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject Player;
    
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        current = 1;
        text2.SetActive(false);
        text3.SetActive(false);
        Player = GameObject.Find("Player");
        Player.transform.position = new Vector3(-0.03f, -4.48f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(current == 1)
            {
                text1.SetActive(false);
                text2.SetActive(true);
                current = 2;
            }
            else if(current == 2)
            {
                text2.SetActive(false);
                text3.SetActive(true);
                current = 3;
            }
        }
    }
}
