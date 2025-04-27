using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    public TimerController timerController; // Timer for the player
    public GameObject halfCheckpoint; // Half checkpoint for the player
    public TextMeshProUGUI lapNumberText; // UI text for the player's lap count

    public int lapCount = 0; // Lap count for the player

    private void Start()
    {
        // Initialize lap count on the UI
        lapNumberText.text = lapCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if Player 1 completed a lap
        if (other.CompareTag("Player") && gameObject.CompareTag("Player1FinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint);
        }

        // Check if AI completed a lap
        if (other.CompareTag("AI") && gameObject.CompareTag("AIFinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint);
        }

        // Check if AI2 completed a lap
        if (other.CompareTag("AI2") && gameObject.CompareTag("AI2FinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint);
        }

        // Check if AI3 completed a lap
        if (other.CompareTag("AI3") && gameObject.CompareTag("AI3FinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint);
        }
    }

    // Handle lap completion for a player
    private void CompleteLap(ref int lapCount, TextMeshProUGUI lapText, TimerController timer, GameObject halfPointTrigger)
    {
        // Activate the halfway point trigger
        halfPointTrigger.SetActive(true);
        this.gameObject.SetActive(false);

        // Check if the lap count has reached 3
        if (lapCount >= 2)
        {
            // Update the best lap time after completing 3 laps
            timer.SetBestTime();
            // Reset the current lap time
            //TimerController.currentTime = 0f;
        }
        // Increment the lap count and update the UI
        lapCount++;
        lapText.text = lapCount.ToString();
    }
}
