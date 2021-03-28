using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryThemeManager : MonoBehaviour
{
    private AudioSource backgroundMusic;
    private AudioClip currentMusic;
    public GameObject[] backgroundMusicObjects;
    private void Awake()
    {
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
    }

    public void PlayMusic()
    {
        backgroundMusic.Play();
    }

    public void StopMusic()
    {
        backgroundMusic.Stop();
    }

    private void changeBackgroundMusic(AudioClip newSong)
    {
        backgroundMusic.clip = newSong;
        currentMusic = newSong;
        PlayMusic();
    }
}
