using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryesType : MonoBehaviour
{
    public GameObject[] tireMeshes; // Array of tire meshes
    public WheelCollider[] wheelColliders; // Array of WheelColliders

    public Vector3 smallTireSize = new Vector3(1f, 1f, 1f); // Scale for small tires
    public Vector3 mediumTireSize = new Vector3(1.5f, 1.5f, 1.5f); // Scale for medium tires
    public Vector3 largeTireSize = new Vector3(2f, 2f, 2f); // Scale for large tires

    private void Update()
    {
        // Sync the wheel meshes with the WheelColliders
        SyncWheelMeshes();
    }

    // Method to sync wheel meshes with WheelColliders
    private void SyncWheelMeshes()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            // Get the WheelCollider's position and rotation
            wheelColliders[i].GetWorldPose(out Vector3 position, out Quaternion rotation);

            // Apply the position and rotation to the corresponding wheel mesh
            tireMeshes[i].transform.position = position;
            tireMeshes[i].transform.rotation = rotation;
        }
    }

    // Method to change tire size
    public void ChangeTireSize(Vector3 newSize, float wheelColliderRadius)
    {
        foreach (GameObject tire in tireMeshes)
        {
            tire.transform.localScale = newSize;
        }

        // Update the WheelCollider radius
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            wheelCollider.radius = wheelColliderRadius;
        }
    }

    // Methods for button clicks
    public void SetSmallTires()
    {
        ChangeTireSize(smallTireSize, 0.5f); // Adjust the radius as needed
    }

    public void SetMediumTires()
    {
        ChangeTireSize(mediumTireSize, 0.75f); // Adjust the radius as needed
    }

    public void SetLargeTires()
    {
        ChangeTireSize(largeTireSize, 1f); // Adjust the radius as needed
    }
}
