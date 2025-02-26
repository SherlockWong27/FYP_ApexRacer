using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAIControl : MonoBehaviour
{
    [SerializeField] private Transform target; // Target to follow waypoint
    [SerializeField] private float steerSensitivity = 0.1f; // Steering sensitivity
    [SerializeField] private float accelSensitivity = 0.1f; // Acceleration sensitivity
    [SerializeField] private float brakeSensitivity = 1f; // Brake sensitivity
    [SerializeField] private float maxSpeed = 20f; // Maximum speed
    [SerializeField] private float distance = 15f; // Distance at which the car starts slowing down
    [SerializeField] private float reduceSpeed = 0.5f; // Speed reduction when approaching the target

    private CarController2 m_CarController;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_CarController = GetComponent<CarController2>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Calculate direction to target
        Vector3 targetDirection = target.position - transform.position;
        float distanceToTarget = targetDirection.magnitude;

        // Calculate steering angle
        float targetAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
        float steer = Mathf.Clamp(targetAngle * steerSensitivity, -1, 1);

        // Calculate desired speed based on distance to target
        float desiredSpeed = maxSpeed;

        // Slow down if the car is close to the target
        if (distanceToTarget < distance)
        {
            float cautiousness = Mathf.InverseLerp(distance, 0, distanceToTarget);
            desiredSpeed = Mathf.Lerp(maxSpeed, maxSpeed * reduceSpeed, cautiousness);
        }

        // Calculate acceleration and braking
        float speed = m_Rigidbody.velocity.magnitude;
        float accel = Mathf.Clamp((desiredSpeed - speed) * accelSensitivity, 0, 1);
        float brake = (speed > desiredSpeed) ? 1 : 0;

        // Move the car
        m_CarController.Move(steer, accel, brake * brakeSensitivity);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
