using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // UI text to display the current lap time
    public TextMeshProUGUI bestTimerText; // UI text to display the best lap time

    public static float currentTime = 0f; // Current lap time
    private float bestTime = Mathf.Infinity; // Best lap time

    private void Update()
    {
        // Update the current lap time
        currentTime += Time.deltaTime;

        // Update the timer display
        UpdateTimerDisplay(timerText, currentTime);
    }

    // Update the timer display in MM:SS:ms format
    private void UpdateTimerDisplay(TextMeshProUGUI text, float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;

        // Format the time as MM:SS:ms
        text.text = $"{minutes:00}:{seconds:00.00}";
    }

    // Set the best lap time if the current lap time is better
    public void SetBestTime()
    {
        // Check if the current lap time is greater than or equal to 10 seconds
        if (currentTime >= 10f && currentTime < bestTime)
        {
            bestTime = currentTime;
            UpdateTimerDisplay(bestTimerText, bestTime);
        }

        // Reset the current lap time
        currentTime = 0f;
    }
}
