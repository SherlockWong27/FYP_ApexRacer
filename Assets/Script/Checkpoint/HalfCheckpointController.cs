using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfCheckpointController : MonoBehaviour
{
    public GameObject finishLine; // The finish line trigger 

    private void OnTriggerEnter(Collider other)
    {
        // Check if Player 1 collided with their half checkpoint
        if (other.CompareTag("Player") && gameObject.CompareTag("Player1HalfCheckpoint"))
        {
            finishLine.SetActive(true);
            this.gameObject.SetActive(false);
        }

        // Check if Player 2 collided with their half checkpoint
        if (other.CompareTag("Player2") && gameObject.CompareTag("Player2HalfCheckpoint"))
        {
            finishLine.SetActive(true);
            this.gameObject.SetActive(false);
        }

        // Check if AI collided with their half checkpoint
        if (other.CompareTag("AI") && gameObject.CompareTag("AIHalfCheckpoint"))
        {
            finishLine.SetActive(true);
            this.gameObject.SetActive(false);
        }

        // Check if AI2 collided with their half checkpoint
        if (other.CompareTag("AI2") && gameObject.CompareTag("AI2HalfCheckpoint"))
        {
            finishLine.SetActive(true);
            this.gameObject.SetActive(false);
        }

        // Check if AI3 collided with their half checkpoint
        if (other.CompareTag("AI3") && gameObject.CompareTag("AI3HalfCheckpoint"))
        {
            finishLine.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
