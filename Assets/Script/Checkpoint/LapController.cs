using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LapController : MonoBehaviour
{
    public TimerController timerController;
    public TimerController timerControllerPlayer2;
    public GameObject halfPointTrigger;
    public TextMeshProUGUI lapNumberText;
    public TextMeshProUGUI lapNumberTextPlayer2;

    private int LapCount = 0;

    private void Start()
    {
        lapNumberText.text = LapCount.ToString();
        lapNumberTextPlayer2.text = LapCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ColliderBottom" && other.CompareTag("Player"))
        {
            halfPointTrigger.SetActive(true);
            this.gameObject.SetActive(false);
            timerController.SetBestTime();
            TimerController.currentTime = 0f;

            LapCount++;
            lapNumberText.text = LapCount.ToString();
        }
        if (other.name == "ColliderBottom" && other.CompareTag("Player2"))
        {
            halfPointTrigger.SetActive(true);
            this.gameObject.SetActive(false);
            timerControllerPlayer2.SetBestTime();
            TimerController.currentTime = 0f;

            LapCount++;
            lapNumberTextPlayer2.text = LapCount.ToString();
        }
    }
}
