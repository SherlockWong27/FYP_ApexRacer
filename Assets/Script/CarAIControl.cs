using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAIControl : MonoBehaviour
{
    [SerializeField] private Transform target; // Target waypoint to follow
    [SerializeField] private float steerSensitivity = 0.1f; // Steering sensitivity
    [SerializeField] private float accelSensitivity = 0.1f; // Acceleration sensitivity
    [SerializeField] private float brakeSensitivity = 1f; // Brake sensitivity
    [SerializeField] private float maxSpeed = 20f; // Maximum speed
    [SerializeField] private float distance = 15f; // Distance at which the car starts slowing down
    [SerializeField] private float reduceSpeed = 0.5f; // Speed reduction when approaching the target

    private CarController m_CarController;
    private Rigidbody m_Rigidbody;
    private AStar pathfinding; // Reference to the Pathfinding script
    private List<Node> path; // List of nodes representing the path
    private int currentPathIndex; // Index of the current target node in the path

    private void Awake()
    {
        m_CarController = GetComponent<CarController>();
        m_Rigidbody = GetComponent<Rigidbody>();
        pathfinding = FindObjectOfType<AStar>(); // Find the Pathfinding script in the scene
    }
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            SetTarget(target);
        }
        if (path == null || path.Count == 0) return;

        // Get the current target node
        Vector3 targetPosition = path[currentPathIndex].worldPosition;

        // Calculate direction to target
        Vector3 targetDirection = targetPosition - transform.position;
        float distanceToTarget = targetDirection.magnitude;

        // Move to the next node if close to the current target node
        if (distanceToTarget < 1f) // Adjust this threshold as needed
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count)
            {
                // Reached the end of the path
                return;
            }
            targetPosition = path[currentPathIndex].worldPosition;
            targetDirection = targetPosition - transform.position;
            distanceToTarget = targetDirection.magnitude;
        }

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

        // Get the path from the AStar script
        if (pathfinding != null)
        {
            path = pathfinding.GetPath(transform.position, target.position);
            currentPathIndex = 0;
        }
    }
}

