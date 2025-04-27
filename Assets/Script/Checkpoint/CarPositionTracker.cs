using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPositionTracker : MonoBehaviour
{
    public string carName = "Player"; // Name of the car
    public RaceManager raceManager; // Reference to the RaceManager

    private float distanceTravelled = 0f;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Calculate distance travelled since the last frame
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        // Update the car's distance in the RaceManager
        for (int i = 0; i < raceManager.cars.Count; i++)
        {
            if (raceManager.cars[i].carName == carName)
            {
                raceManager.cars[i].distanceTravelled = distanceTravelled;
                break;
            }
        }
    }
}
