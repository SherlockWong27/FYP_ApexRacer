using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CarController carController;
    public string inputID;
    private float steeringInput; // Steering input (-1 to 1)
    private float accelerationInput; // Acceleration input (0 to 1)
    private float brakeInput; // Brake input (0 to 1)
    [SerializeField] private Camera backCamera; // Back camera 
    [SerializeField] private Camera frontCamera; // Front camera 
    private bool switchCamera = true; // Track which camera is active
    private void Awake()
    {
        carController = GetComponent<CarController>();
        // Ensure the default camera is active and the alternate camera is inactive at start
        if (backCamera != null && frontCamera != null)
        {
            backCamera.gameObject.SetActive(true);
            frontCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Only process input if the script is enabled
        if (enabled)
        {
            accelerationInput = Input.GetAxis("Vertical" + inputID);
            steeringInput = Input.GetAxis("Horizontal" + inputID);

            // Set brake input based on the inputID
            if (inputID == "1")
            {
                brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
                // Switch camera 
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SwitchCamera();
                }
            }
            else if (inputID == "2")
            {
                brakeInput = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
                // Switch camera 
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SwitchCamera();
                }
            }
        }
        else
        {
            // If the script is disabled, set all inputs to zero
            accelerationInput = 0f;
            steeringInput = 0f;
            brakeInput = 0f;
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

    // Method to switch between the back and front cameras
    private void SwitchCamera()
    {
        if (backCamera != null && frontCamera != null)
        {
            switchCamera = !switchCamera;
            backCamera.gameObject.SetActive(switchCamera);
            frontCamera.gameObject.SetActive(!switchCamera);
        }
    }
}
