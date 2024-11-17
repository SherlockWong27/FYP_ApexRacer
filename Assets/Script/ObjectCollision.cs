using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    public GameObject frontCarPart; // The front part of the car (could be a mesh or an empty object containing parts)
    public GameObject[] tires;      // Array for tires that might fall off
    public float impactThreshold = 5f;  // Minimum collision force required to trigger parts detaching

    // Start is called before the first frame update
    void Start()
    {
        // Make sure each part has a Rigidbody and is initially set as Kinematic (not affected by physics)
        foreach (GameObject tire in tires)
        {
            tire.GetComponent<Rigidbody>().isKinematic = true;
        }

        frontCarPart.GetComponent<Rigidbody>().isKinematic = true;
    }

    // This method is called when a collision happens
    void OnCollisionEnter(Collision collision)
    {
        // Check the impact force to determine if the parts should fall off
        if (collision.relativeVelocity.magnitude > impactThreshold)
        {
            // Detach the front part of the car
            DetachPart(frontCarPart);

            // Detach the tires
            foreach (GameObject tire in tires)
            {
                DetachPart(tire);
            }
        }
    }

    // Detaches a part from the car
    void DetachPart(GameObject part)
    {
        // Set the part to be non-kinematic so that physics can affect it
        Rigidbody rb = part.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics interactions
            rb.transform.parent = null; // Detach it from the parent car object

            // Optionally, you can apply some force to simulate the "falling off" effect
            rb.AddForce(Vector3.up * 100f); // Add upward force to simulate the part falling off
        }
    }
}
