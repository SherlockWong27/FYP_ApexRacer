using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false; // Track whether the game is paused
    //Getter function to get the instance of the class
    private static PauseManager instance = null;
    public static PauseManager instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        // Toggle pause when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }
    }

    // Toggle pause state
    public void Paused()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    // Pause the game
    private void PauseGame()
    {
        Time.timeScale = 0; // Stop time
        PausedMenu.instanceClass.SetStatus(true); // Show the pause menu
    }

    // Resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume time
        PausedMenu.instanceClass.SetStatus(false); // Hide the pause menu
    }
}
