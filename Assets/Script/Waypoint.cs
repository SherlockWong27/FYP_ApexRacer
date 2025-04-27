using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<GameObject> waypoints;// List of waypoints positions

    private int tracker = 0;// Tracks which waypoints 
    private void Awake()
    {
        // Move to the first waypoint
        this.transform.position = waypoints[0].transform.position;
        this.transform.rotation = waypoints[0].transform.rotation;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ColliderBottom" && other.CompareTag("AI") || other.name == "ColliderBottom" && other.CompareTag("AI2") || other.name == "ColliderBottom" && other.CompareTag("AI3"))
        {
            // Move to the next waypoint
            tracker += 1;
            // Hide the waypoint
            this.gameObject.SetActive(false);
            // Reset back to the first waypoint after the reaching 8 waypoints
            if (tracker == 8)
            {
                tracker = 0;
            }
            // Update position and rotation to the next waypoint
            this.transform.position = waypoints[tracker].transform.position;
            this.transform.rotation = waypoints[tracker].transform.rotation;
            this.gameObject.SetActive(true);
        }
    }
}
