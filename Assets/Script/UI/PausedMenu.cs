using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PausedMenu : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    [SerializeField] private Button backButton = null; // Close the elements
    //Getter function to get the instance of the class
    private static PausedMenu instance = null;
    public static PausedMenu instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        elements.SetActive(false);
    }
    public void SetStatus(bool status)
    {
        elements.SetActive(status);
    }

    private void Start()
    {
        // Add listeners to the buttons
        backButton.onClick.AddListener(BackMainMenu);
    }

    private void BackMainMenu()
    {
        elements.SetActive(false);
        MainMenu.instanceClass.SetStatus(true);
        OnePlayer.instanceClass.SetStatus(false);
        TwoPlayer.instanceClass.SetStatus(false);
        PauseManager.instanceClass.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
