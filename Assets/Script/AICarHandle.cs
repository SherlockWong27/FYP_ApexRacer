using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICarHandle : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints, followMouse };

    [Header("AI settings")]
    public AIMode aiMode;
    public float maxSpeed = 16;
    public bool isAvoidingCars = true;
    [Range(0.0f, 1.0f)]
    public float skillLevel = 1.0f;

    private Vector3 targetPosition = Vector3.zero;
    private Transform targetTransform = null;
    private float originalMaximumSpeed = 0;

    private Vector2 avoidanceVectorLerped = Vector3.zero;

    private WaypointNode currentWaypoint = null;
    private WaypointNode previousWaypoint = null;
    private WaypointNode[] allWayPoints;

    private CarController carController;

    void Awake()
    {
        carController = GetComponent<CarController>();
        allWayPoints = FindObjectsOfType<WaypointNode>();
        if (allWayPoints.Length == 0)
        {
            Debug.LogError("No waypoint nodes found in the scene!");
        }
        originalMaximumSpeed = maxSpeed;
    }

    void Start()
    {
        SetMaxSpeedBasedOnSkillLevel(maxSpeed);
    }

    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;
        /*
        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;

            case AIMode.followWaypoints:
                FollowWaypoints();
                break;

            case AIMode.followMouse:
                FollowMousePosition();
                break;
        }*/
        FollowWaypoints();

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        carController.SetInputVector(inputVector);
    }

    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }

    void FollowWaypoints()
    {
        //Pick the cloesest waypoint if we don't have a waypoint set.
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWayPoint();
            previousWaypoint = currentWaypoint;
            Debug.Log("Starting at waypoint: " + currentWaypoint.name);
        }

        //Set the target on the waypoints position
        if (currentWaypoint != null)
        {
            Debug.Log("Current waypoint: " + currentWaypoint.name);
            //currentWaypoint = FindClosestWayPoint();
            //Set the target position of for the AI. 
            targetPosition = currentWaypoint.transform.position;
            // Set the target position for the AI, ignoring the Y-axis
            //targetPosition = new Vector3(currentWaypoint.transform.position.x, transform.position.y, currentWaypoint.transform.position.z);


            //Store how close we are to the target
            float distanceToWayPoint = (targetPosition - transform.position).magnitude;

            //Check if we are close enough to consider that we have reached the waypoint
            if (distanceToWayPoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                Debug.Log("Reached waypoint: " + currentWaypoint.name);
                if (currentWaypoint.maxSpeed > 0)
                    SetMaxSpeedBasedOnSkillLevel(currentWaypoint.maxSpeed);
                else SetMaxSpeedBasedOnSkillLevel(1000);

                //Store the current waypoint as previous before we assign a new current one.
                previousWaypoint = currentWaypoint;

                //If we are close enough then follow to the next waypoint, if there are multiple waypoints then pick one at random.
                //currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
                if (currentWaypoint.nextWaypointNode.Length > 0)
                {
                    currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
                }
                else
                {
                    Debug.LogError($"No valid next waypoint found for {currentWaypoint.name}");
                }
            }
            else
            {
                Debug.Log($"Current Waypoint: {currentWaypoint.name}, Position: {currentWaypoint.transform.position}");
            }

            //Debug.Log($"Distance to Waypoint: {distanceToWayPoint}");
        }

    }

    void FollowMousePosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = worldPosition;
    }

    WaypointNode FindClosestWayPoint()
    {
        // Find the closest waypoint, ignoring the Y-axis
        return allWayPoints
            .OrderBy(t => Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(t.transform.position.x, 0, t.transform.position.z)))
            .FirstOrDefault();
    }

    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();


        //Calculate an angle towards the target 
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        //We want the car to turn as much as possible if the angle is greater than 45 degrees and we wan't it to smooth out so if the angle is small we want the AI to make smaller corrections. 
        float steerAmount = angleToTarget / 45.0f;

        //Clamp steering to between -1 and 1.
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    float ApplyThrottleOrBrake(float inputX)
    {
        if (carController.GetVelocityMagnitude() > maxSpeed)
            return 0;

        float reduceSpeedDueToCornering = Mathf.Abs(inputX) / 1.0f;
        return 1.05f - reduceSpeedDueToCornering * skillLevel;
    }

    void SetMaxSpeedBasedOnSkillLevel(float newSpeed)
    {
        maxSpeed = Mathf.Clamp(newSpeed, 0, originalMaximumSpeed);
        maxSpeed *= Mathf.Clamp(skillLevel, 0.3f, 1.0f);
    }
}
