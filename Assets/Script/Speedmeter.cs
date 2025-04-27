using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speedmeter : MonoBehaviour
{
    public TextMeshProUGUI Speed_Text; // Reference to the UI text for displaying speed
    public CarController carController; // Reference to the CarController script
    public bool Speed_In_KPH = true; // Display speed in km/h (true) or mph (false)

    private void Update()
    {
        // If Speed_Text and carController are assigned
        if (Speed_Text != null && carController != null)
        {
            // Get the car's speed from the CarController
            float speed = carController.GetSpeed();

            // Convert speed to km/h or mph based on the Speed_In_KPH flag
            if (Speed_In_KPH)
            {
                speed *= 3.6f; // Convert m/s to km/h
            }
            else
            {
                speed *= 2.23694f; // Convert m/s to mph
            }

            // Round the speed to an integer and display it
            int speedToDisplay = Mathf.RoundToInt(speed);
            Speed_Text.SetText(speedToDisplay.ToString() + (Speed_In_KPH ? " km/h" : " mph"));
        }
    }
}
