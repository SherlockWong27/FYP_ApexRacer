using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Assign this in the Inspector
    private float elapsedTime;
    public bool isRunning = false;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:D2}:{1:D2}.{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
