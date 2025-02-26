using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapControllerOnePlayer : MonoBehaviour
{
    public TimerController timerController;
    public GameObject halfPointTrigger;
    public TextMeshProUGUI lapNumberText;

    private int LapCount = 0;

    private void Start()
    {
        lapNumberText.text = LapCount.ToString();
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
    }
}
