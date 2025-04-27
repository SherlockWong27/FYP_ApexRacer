using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinishGame : MonoBehaviour
{
    [SerializeField] public GameObject elements = null; // Finish game UI elements
    [SerializeField] private Button backButton = null; // Button to return to the main menu
    [SerializeField] private TextMeshProUGUI leaderboardText = null; // UI text to display the leaderboard

    private static FinishGame instance = null;
    public static FinishGame instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // Set the singleton instance
        instance = this;

        // Hide the finish screen by default
        elements.SetActive(false);
    }

    private void Start()
    {
        // Add listener to the back button
        backButton.onClick.AddListener(BackMainMenu);
    }

    // Method to show or hide the finish screen
    public void SetStatus(bool status)
    {
        elements.SetActive(status);

        // If the finish screen is being shown, update the leaderboard
        if (status)
        {
            UpdateLeaderboard();
        }
    }

    // Method to update the leaderboard UI
    private void UpdateLeaderboard()
    {
        // Get the leaderboard data from the RaceManager
        RaceManager raceManager = FindObjectOfType<RaceManager>();
        if (raceManager != null)
        {
            List<CarPosition> leaderboard = raceManager.GetLeaderboard();
            // Update the leaderboard text
            string leaderboardString = "Leaderboard:\n";
            for (int i = 0; i < leaderboard.Count; i++)
            {
                string bestLapTime = leaderboard[i].timerController != null ?
                    leaderboard[i].timerController.GetBestTimeFormatted() : "";

                leaderboardString += $"{i + 1}. {leaderboard[i].carName} - Best Time: {bestLapTime}\n";
            }
            leaderboardText.text = leaderboardString;
        }
    }
    // Method to return to the main menu
    private void BackMainMenu()
    {
        elements.SetActive(false);
        MainMenu.instanceClass.SetStatus(true);
        OnePlayer.instanceClass.SetStatus(false);
        TwoPlayer.instanceClass.SetStatus(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
