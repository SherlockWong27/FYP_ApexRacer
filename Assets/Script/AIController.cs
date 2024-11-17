using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;
    // Speed modifiers
    public float normalSpeed = 10f;
    public float cornerSpeed = 3f;
    public float turnAngleThreshold = 45f; // Angle in degrees to detect a sharp turn

    private Vector3 previousDestination;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = normalSpeed;  // Set the normal speed initially
        previousDestination = agent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                previousDestination = agent.destination;
            }
        }

        AdjustSpeedAtCorners();
    }

    void AdjustSpeedAtCorners()
    {
        // Check if the car is turning
        Vector3 currentDirection = agent.velocity.normalized;
        Vector3 targetDirection = (agent.destination - agent.transform.position).normalized;

        // Calculate the angle between the current direction and the target direction
        float angle = Vector3.Angle(currentDirection, targetDirection);

        if (angle > turnAngleThreshold)
        {
            // If the angle is greater than the threshold, slow down
            agent.speed = cornerSpeed;
        }
        else
        {
            // Otherwise, use the normal speed
            agent.speed = normalSpeed;
        }
    }
}
