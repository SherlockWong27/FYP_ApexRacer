using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
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
    private bool isFlipped; // Track if the car is flipped
    private float flipStartTime; // Time when the car was flipped

    [SerializeField] private TrailRenderer[] tireMarks; // Trail Renderers for tire marks
    [SerializeField] private ParticleSystem[] tireParticles; // Particle Systems for tire effects
    [SerializeField] private float minSpeedForParticles = 5f; // Minimum speed to activate particles

    [SerializeField] private GameObject[] brakeLights; // Array of brake light objects
    [SerializeField] private float brakeLightIntensity = 5f; // Intensity of brake lights when active

    [SerializeField] private AudioSource engineAudioSource; // AudioSource for engine sound
    [SerializeField] private AudioSource driftAudioSource; // AudioSource for drifting sound
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGrip = wheelColliders[0].sidewaysFriction.stiffness; // Store original grip
        for (int i = 0; i < 4; i++)
        {
            tireMarks[i].emitting = false;
            if (tireParticles[i] != null)
            {
                tireParticles[i].Stop(); // Stop particles initially
            }
        }
        // Initialize brake lights
        SetBrakeLights(false);

        // Initialize audio sources
        if (engineAudioSource != null)
        {
            engineAudioSource.loop = true; // Loop the engine sound
            engineAudioSource.Play();
        }

        if (driftAudioSource != null)
        {
            driftAudioSource.loop = true; // Loop the drifting sound
        }
    }
    private void Update()
    {
        // Check if the car is flipped and reset it if necessary
        CheckAndResetFlip();

        // Enable/disable tire marks based on drift
        UpdateTireMarks();
        UpdateTireParticles();
        // Control engine sound based on car speed
        UpdateEngineSound();

        // Control drifting sound based on drift state
        UpdateDriftSound();
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
        // Control brake lights based on brake input
        SetBrakeLights(brake > 0);
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
    // Apply drifting
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
    // Method to get the car speed
    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    // Method to check if the car is flipped and reset it
    private void CheckAndResetFlip()
    {
        // Check if the car is flipped over 90 or -90 or 180 degrees
        if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) > 0.5f || Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward)) > 0.5f || Vector3.Dot(transform.up, Vector3.down) > 0.5f)
        {
            if (!isFlipped)
            {
                // Start the flip timer
                isFlipped = true;
                flipStartTime = Time.time;
            }
            else if (Time.time - flipStartTime >= 5f)
            {
                // If 5 seconds have passed, reset the car
                ResetCar();
                isFlipped = false; // Reset the flipped state
            }
        }
        else
        {
            // If the car is no longer flipped, reset the flipped state
            isFlipped = false;
        }
    }

    // Method to reset the car position and rotation
    private void ResetCar()
    {
        // Reset the car rotation to upright
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Reset the car velocity and angular velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Method to enable/disable tire marks based on drift
    private void UpdateTireMarks()
    {
        bool isDrifting = IsDrifting();

        for (int i = 0; i < 4; i++)
        {
            tireMarks[i].emitting = isDrifting;
        }
    }

    // Method to check if the car is drifting
    private bool IsDrifting()
    {
        // Check if the car is sliding sideways
        float sidewaysSpeed = Vector3.Dot(rb.velocity, transform.right);
        return Mathf.Abs(sidewaysSpeed) > 2f; // Adjust threshold as needed
    }

    // Method to enable/disable tire particles based on speed
    private void UpdateTireParticles()
    {
        bool isMoving = GetSpeed() > minSpeedForParticles;

        for (int i = 0; i < 4; i++)
        {
            if (tireParticles[i] != null)
            {
                if (isMoving && !tireParticles[i].isPlaying)
                {
                    tireParticles[i].Play(); // Start particles if the car is moving
                }
                else if (!isMoving && tireParticles[i].isPlaying)
                {
                    tireParticles[i].Stop(); // Stop particles if the car is not moving
                }
            }
        }
    }

    // Method to control brake lights
    private void SetBrakeLights(bool isBraking)
    {
        foreach (GameObject brakeLight in brakeLights)
        {
            // If the brake light has a Light component, adjust its intensity
            Light lightComponent = brakeLight.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.intensity = isBraking ? brakeLightIntensity : 0f;
            }

            // If the brake light has a MeshRenderer, enable/disable it
            MeshRenderer renderer = brakeLight.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = isBraking;
            }
        }
    }

    // Method to control engine sound based on car speed
    private void UpdateEngineSound()
    {
        if (engineAudioSource != null)
        {
            // Adjust the pitch of the engine sound based on car speed
            float speed = GetSpeed();
            engineAudioSource.pitch = Mathf.Lerp(0.8f, 1.2f, speed / 20f); // Adjust pitch range as needed
        }
    }

    // Method to control drifting sound based on drift state
    private void UpdateDriftSound()
    {
        if (driftAudioSource != null)
        {
            if (IsDrifting())
            {
                if (!driftAudioSource.isPlaying)
                {
                    driftAudioSource.Play(); // Play the drifting sound
                }
            }
            else
            {
                if (driftAudioSource.isPlaying)
                {
                    driftAudioSource.Stop(); // Stop the drifting sound
                }
            }
        }
    }
}