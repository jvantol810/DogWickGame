using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource backgroundMusic;
    public AudioClip gameOverMusic;
    private AudioClip currentMusic;
    public GameObject[] backgroundMusicObjects;
    Scene currentScene;
    private void Awake()
    {
        backgroundMusicObjects = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        foreach (GameObject backgroundMusic in backgroundMusicObjects)
        {
            if (backgroundMusic.GetInstanceID() != gameObject.GetInstanceID())
            {
                GameObject.Destroy(backgroundMusic);
            }
        }
        DontDestroyOnLoad(transform.gameObject);
        backgroundMusic = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentMusic = backgroundMusic.clip;
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Room9")
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        backgroundMusic.Play();
    }

    public void StopMusic()
    {
        backgroundMusic.Stop();
    }

    public void playGameOverMusic()
    {
        changeBackgroundMusic(gameOverMusic);
        PlayMusic();
    }
    private void changeBackgroundMusic(AudioClip newSong)
    {
        backgroundMusic.clip = newSong;
        currentMusic = newSong;
        PlayMusic();
    }
}
