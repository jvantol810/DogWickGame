using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    float killedEnemies;
    float amountOfEnemies;
    float playerHealth;
    float playerHealthFromPreviousRoom;
    public GameObject Player;
    public GameObject Door;
    public GameObject LoyaltyUIObject;
    public BackgroundMusicController musicController;
    public GameObject OpenUpperDoor;
    public GameObject OpenLowerDoor;
    public GameObject Exit;
    private GameObject[] enemies;
    private GameObject[] wallEdges;
    private GameObject OpenDoor;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        amountOfEnemies = enemies.Length;
        //SET EACH ENEMY IN THE ROOM'S Z POSITION TO 0
        for(int i = 0; i < enemies.Length; i++)
        {
            Vector3 enemyPosition = enemies[i].GetComponent<Transform>().position;
            Vector3 newPosition = new Vector3(enemyPosition.x, enemyPosition.y, 0);
            enemies[i].GetComponent<Transform>().SetPositionAndRotation(newPosition, Quaternion.identity);
        }

        wallEdges = GameObject.FindGameObjectsWithTag("WallEdge");
        //SET EACH WALLEDGE IN THE ROOM'S Z POSITION TO 0
        for (int i = 0; i < wallEdges.Length; i++)
        {
            Vector3 wallEdgePosition = wallEdges[i].GetComponent<Transform>().position;
            Vector3 newWallEdgePosition = new Vector3(wallEdgePosition.x, wallEdgePosition.y, 0);
            wallEdges[i].GetComponent<Transform>().SetPositionAndRotation(newWallEdgePosition, Quaternion.identity);
        }

        OpenDoor = GameObject.Find("OpenDoor");
        Vector3 openDoorPosition = OpenDoor.GetComponent<Transform>().position;
        Vector3 newOpenDoorPosition = new Vector3(openDoorPosition.x, openDoorPosition.y, 0);
        OpenDoor.GetComponent<Transform>().SetPositionAndRotation(newOpenDoorPosition, Quaternion.identity);

        Player = GameObject.Find("Player");
        Door = GameObject.Find("Door");
        OpenUpperDoor = GameObject.Find("OpenUpperDoor");
        OpenLowerDoor = GameObject.Find("OpenLowerDoor");
        Exit = GameObject.Find("Exit");
        killedEnemies = 0;
        
        //Player.SetActive(true);
        Player.transform.position = new Vector3(-0.03f, -4.48f, 0);
        Player.GetComponent<PlayerController>().kills = 0;
        LoyaltyUIObject = GameObject.Find("LoyaltyLabel");
        //LoyaltyUIObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player"); 
        killedEnemies = Player.GetComponent<PlayerController>().kills;
        if(killedEnemies == amountOfEnemies)
        {
            openDoor();
        }
        playerHealth = Player.GetComponent<PlayerController>().health;
        if (playerHealth <= 0)
        {
            gameOver();
        }
        
        
    }

    public void addKill()
    {
        killedEnemies++;
    }

    public void gameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void openDoor()
    {
        //Before opening door, store player's health inside the PlayerHealthFromPreviousRoom variable
        playerHealthFromPreviousRoom = Player.GetComponent<PlayerController>().health;
        OpenUpperDoor.GetComponent<SpriteRenderer>().enabled = true;
        OpenLowerDoor.GetComponent<SpriteRenderer>().enabled = true;
        Exit.GetComponent<BoxCollider2D>().enabled = true;
    }
}
