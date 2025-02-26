using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CarController2 carController;
    public string inputID;
    private float steeringInput; // Steering input (-1 to 1)
    private float accelerationInput; // Acceleration input (0 to 1)
    private float brakeInput; // Brake input (0 to 1)
    private void Awake()
    {
        carController = GetComponent<CarController2>();
    }

    private void Update()
    {
        accelerationInput = Input.GetAxis("Vertical" + inputID);
        steeringInput = Input.GetAxis("Horizontal" + inputID);
        // Set brake input based on the inputID
        if (inputID == "1") 
        {
            brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f; 
        }
        else if (inputID == "2") 
        {
            brakeInput = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f; 
        }
    }
    private void FixedUpdate()
    {
        // Pass input to the CarController
        if (carController != null)
        {
            carController.Move(steeringInput, accelerationInput, brakeInput);
        }
    }
}
