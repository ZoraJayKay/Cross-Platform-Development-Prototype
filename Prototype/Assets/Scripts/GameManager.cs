using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    public Settings settings;
    private AudioSource backgroundMusic;
    private GameObject musicObject;

    public void Start()
    {
        // Make the screen landscape from the moment the game starts
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        musicObject = GameObject.FindWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        // If the user presses Esc, quit the program
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            Application.Quit();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        backgroundMusic = musicObject.GetComponent<BackgroundMusic>().currentSource;

        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
            if (backgroundMusic.volume > 0.25f) { backgroundMusic.volume = 0.25f; }            
        }

        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            backgroundMusic.volume = settings.musicVolume / 100;
        }        
    }
}
