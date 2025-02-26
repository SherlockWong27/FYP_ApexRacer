using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public Vector3 offset = new Vector3(0f, 3.5f, -7f); // Camera offset from the player
    public float smoothSpeed = 1f; // Smoothing speed for camera movement

    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 position = player.transform.position + player.transform.TransformDirection(offset);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, position, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the player
        transform.LookAt(player.transform);
    }
}
