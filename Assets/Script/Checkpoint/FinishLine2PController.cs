using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine2PController : MonoBehaviour
{
    public TimerController timerController; // Timer for the player
    public GameObject halfCheckpoint; // Half checkpoint
    public TextMeshProUGUI lapNumberText; // UI text for the player's lap count
    public PlayerInput playerInput; // Reference to the PlayerInput script
    public int lapCount = 0; // Lap count for the player
    private void OnTriggerEnter(Collider other)
    {
        // Check if Player 1 completed a lap
        if (other.CompareTag("Player") && gameObject.CompareTag("Player1FinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint, other.GetComponent<PlayerInput>());
        }

        // Check if Player 2 completed a lap
        if (other.CompareTag("Player2") && gameObject.CompareTag("Player2FinishLine"))
        {
            CompleteLap(ref lapCount, lapNumberText, timerController, halfCheckpoint, other.GetComponent<PlayerInput>());
        }
    }

    // Handle lap completion for a player
    private void CompleteLap(ref int lapCount, TextMeshProUGUI lapText, TimerController timer, GameObject halfPointTrigger, PlayerInput playerInput)
    {
        // Activate the halfway point trigger
        halfPointTrigger.SetActive(true);
        this.gameObject.SetActive(false);

        // Check if the lap count has reached 3
        if (lapCount >= 2)
        {
            // Update the best lap time after completing 3 laps
            timer.SetBestTime();
        }
        // Increment the lap count and update the UI
        lapCount++;
        lapText.text = lapCount.ToString();

        // Check if the player has completed 3 laps
        if (lapCount >= 3)
        {
            // Disable the player's input
            if (playerInput != null)
            {
                playerInput.enabled = false; // Disable the PlayerInput script
                Debug.Log("Player has completed 3 laps. Disabling input.");
            }
        }
    }
}
