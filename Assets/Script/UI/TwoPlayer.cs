using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TwoPlayer : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    [SerializeField] private Button sunnyButton = null; // Button for sunny weather
    [SerializeField] private Button rainButton = null; // Button for rainy weather
    [SerializeField] private ParticleSystem rainParticleSystem = null; // Reference to the rain particle system
    [SerializeField] private PuddlerSpawner puddleSpawner = null; // Reference to the puddle spawner
    //Getter function to get the instance of the class
    private static TwoPlayer instance = null;
    public static TwoPlayer instanceClass
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
        // Ensure the rain effect is disabled at the start
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Stop();
        }
    }
    public void SetStatus(bool status)
    {
        elements.SetActive(status);
    }
    private void Start()
    {
        // Add listeners to the buttons
        sunnyButton.onClick.AddListener(StopRain);
        rainButton.onClick.AddListener(StartRain);
    }

    // Start the rain effect
    private void StartRain()
    {
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Play(); // Play the rain particle system
        }
        if (puddleSpawner != null)
        {
            puddleSpawner.SpawnPuddles(); // Spawn puddles when it starts raining
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
    }
}
