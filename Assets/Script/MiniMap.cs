using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform Target; // Target (the car)

    public Transform Icon; // The icon of the car
    public Vector3 Icon_Rotation_Offset; // The icon rotation offset
    public float Height_Offset; // How high the icon is above the car

    void Update()
    {
        // Update the icon's position and rotation to follow the player
        if (Target != null && Icon != null)
        {
            // Set the icon's position to the player's position with a height offset
            Icon.position = Target.position + new Vector3(0, Height_Offset, 0);

            // Set the icon's rotation to match the player's rotation with an offset
            Icon.rotation = Target.rotation * Quaternion.Euler(Icon_Rotation_Offset);
        }
    }
}
