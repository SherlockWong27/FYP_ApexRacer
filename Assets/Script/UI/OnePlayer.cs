using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnePlayer : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    [SerializeField] public GameObject rain = null;
    [SerializeField] private Button sunnyButton = null; // Button for sunny weather
    [SerializeField] private Button rainButton = null; // Button for rainy weather
    [SerializeField] private ParticleSystem rainParticleSystem = null; // Reference to the rain particle system
    [SerializeField] private PuddlerSpawner puddleSpawner = null; // Reference to the puddle spawner
    [SerializeField] private Light directionalLight; // Reference to the directional light
    [SerializeField] private AudioSource rainAudioSource; // AudioSource for rain sound
    //Getter function to get the instance of the class
    private static OnePlayer instance = null;
    public static OnePlayer instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        elements.SetActive(false);
        rain.SetActive(false);
        // Ensure the rain effect is disabled at the start
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Stop();
        }
        // Reset the current lap time
        TimerController.currentTime = 0f;
    }
    public void SetStatus(bool status)
    {
        elements.SetActive(status);
    }
    public void SetStatusPlayGame(bool status)
    {
        elements.SetActive(status);
    }

    private void Start()
    {
        // Add listeners to the buttons
        sunnyButton.onClick.AddListener(StopRain);
        rainButton.onClick.AddListener(StartRain);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.instanceClass.SetStatus(true);
            SetStatus(false);
        }

        // Play the rain sound
        if (rainAudioSource != null)
        {
            rainAudioSource.loop = true; //loop the rain sound
        }
    }

    // Start the rain effect
    private void StartRain()
    {
        rain.SetActive(true);
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Play(); // Play the rain particle system
        }
        if (puddleSpawner != null)
        {
            puddleSpawner.SpawnPuddles(); // Spawn puddles when it starts raining
        }
        // Change the light to darker
        if (directionalLight != null)
        {
            directionalLight.intensity = 0.01f; // Adjust this value to make the light darker
        }
        // Play the rain sound
        if (rainAudioSource != null)
        {
            rainAudioSource.Play(); // Play the rain sound
        }

    }

    // Stop the rain effect
    private void StopRain()
    {
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Stop(); // Stop the rain particle system
        }
        if (puddleSpawner != null)
        {
            puddleSpawner.RemovePuddles(); // Remove puddles when it stops raining
        }
        // Reset the light intensity
        if (directionalLight != null)
        {
            directionalLight.intensity = 1f; // Reset to the original intensity
        }
        // Stop the rain sound
        if (rainAudioSource != null)
        {
            rainAudioSource.Stop(); // Stop the rain sound
        }
    }
}
