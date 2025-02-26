using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float brakeTorque = 3000f;
    public float maxSpeed = 50f;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Transforms")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    private float motorInput;
    private float steeringInput;
    private float brakeInput;

    private Rigidbody rb;
    private Vector2 inputVector = Vector2.zero;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        HandleMotor();
        HandleSteering();
        UpdateWheelTransforms();
        LimitSpeed();
    }

    private void HandleMotor()
    {
        float motorForce = motorInput * maxMotorTorque;

        // Apply motor torque to rear wheels
        rearLeftWheel.motorTorque = motorForce;
        rearRightWheel.motorTorque = motorForce;

        // Apply brake torque if brake input is provided
        if (brakeInput > 0)
        {
            rearLeftWheel.brakeTorque = brakeInput * brakeTorque;
            rearRightWheel.brakeTorque = brakeInput * brakeTorque;
        }
        else
        {
            rearLeftWheel.brakeTorque = 0f;
            rearRightWheel.brakeTorque = 0f;
        }
    }

    private void HandleSteering()
    {
        float steeringAngle = steeringInput * maxSteeringAngle;

        // Apply steering angle to front wheels
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }

    private void UpdateWheelTransforms()
    {
        UpdateWheelTransform(frontLeftWheel, frontLeftTransform);
        UpdateWheelTransform(frontRightWheel, frontRightTransform);
        UpdateWheelTransform(rearLeftWheel, rearLeftTransform);
        UpdateWheelTransform(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheelTransform(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void SetInput(float motor, float steering, float brake)
    {
        motorInput = motor;
        steeringInput = steering;
        brakeInput = brake;
    }

    // Set the input vector for acceleration and turning
    public void SetInputVector(Vector2 input)
    {
        inputVector = input;
    }

    // Returns the current velocity magnitude of the car
    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    private void ApplyMovement()
    {
        // Apply movement and steering based on input vector
        float forwardSpeed = inputVector.y * 20f; // Example acceleration multiplier
        float turnSpeed = inputVector.x * 50f;   // Example turn multiplier

        // Apply forward movement
        Vector3 forwardForce = transform.forward * forwardSpeed;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

    }
}
