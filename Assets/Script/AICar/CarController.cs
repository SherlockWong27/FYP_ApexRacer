using UnityEngine;

public class CarController2 : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4]; // Wheel colliders
    [SerializeField] private GameObject[] wheelMeshes = new GameObject[4]; // Wheel meshes
    [SerializeField] private float maxSteerAngle = 60f; // Maximum steering angle
    [SerializeField] private float motorTorque = 1000f; // Motor torque for acceleration
    [SerializeField] private float brake = 2000f; // Brake torque for braking
    [SerializeField] private float drift = 0.5f; // Drift stiffness for sliding
    [SerializeField] private float puddleSlip = 0.2f; // Slip factor when in a puddle
    private Rigidbody rb;
    private bool insidePuddle; // Track if the car is in a puddle
    private float originalGrip; // Store the original wheel grip
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGrip = wheelColliders[0].sidewaysFriction.stiffness; // Store original grip
    }

    public void Move(float steering, float accel, float brake)
    {
        // Update wheel positions and rotations
        for (int i = 0; i < 4; i++)
        {
            Quaternion rotation;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out rotation);
            wheelMeshes[i].transform.position = position;
            wheelMeshes[i].transform.rotation = rotation;
        }

        // Steering
        wheelColliders[0].steerAngle = steering * maxSteerAngle;
        wheelColliders[1].steerAngle = steering * maxSteerAngle;

        // Acceleration
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = accel * motorTorque;
        }

        // Braking 
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].brakeTorque = brake * this.brake;
        }

        // Apply drift or puddle effects
        if (insidePuddle)
        {
            ApplyPuddleEffect();
        }
        else
        {
            // Drift effect 
            ApplyDrift();
        }

    }

    private void ApplyDrift()
    {
        WheelFrictionCurve friction = wheelColliders[0].sidewaysFriction;
        friction.stiffness = drift;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].sidewaysFriction = friction;
        }
    }

    private void ApplyPuddleEffect()
    {
        // Adjust wheel for increased sliding in puddles
        WheelFrictionCurve friction = wheelColliders[0].sidewaysFriction;
        friction.stiffness = puddleSlip;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].sidewaysFriction = friction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the car entered a puddle
        if (other.CompareTag("Puddle"))
        {
            insidePuddle = true;
            Debug.Log("Car entered puddle!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the car exited a puddle
        if (other.CompareTag("Puddle"))
        {
            insidePuddle = false;

            // Reset wheel grip to normal
            WheelFrictionCurve friction = wheelColliders[0].sidewaysFriction;
            friction.stiffness = originalGrip;

            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].sidewaysFriction = friction;
            }
        }
    }
    // Method to get the car's speed
    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }
}