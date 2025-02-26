using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddlerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject puddlePrefab; // Puddle prefab to spawn
    [SerializeField] private int numberOfPuddles = 10; // Number of puddles to spawn
    [SerializeField] private Vector3 minSpawnArea = new Vector3(150, 0, 165); // Minimum bounds of the spawn area
    [SerializeField] private Vector3 maxSpawnArea = new Vector3(10, 0, -80); // Maximum bounds of the spawn area
    [SerializeField] private float yOffset = 1f; // Y offset to place puddles slightly above the ground
    private List<GameObject> spawnedPuddles = new List<GameObject>(); // List to track spawned puddles
    public void SpawnPuddles()
    {
        // Clear existing puddles 
        RemovePuddles();

        for (int i = 0; i < numberOfPuddles; i++)
        {
            // Calculate a random position within the min and max spawn area
            Vector3 randomPosition = new Vector3(Random.Range(minSpawnArea.x, maxSpawnArea.x)
                ,yOffset,Random.Range(minSpawnArea.z, maxSpawnArea.z));
            // Spawn the puddle at the random position
            GameObject puddle = Instantiate(puddlePrefab, randomPosition, Quaternion.identity, transform);
            spawnedPuddles.Add(puddle); // Add the puddle to the list
        }
    }

    public void RemovePuddles()
    {
        // Destroy all spawned puddles
        foreach (GameObject puddle in spawnedPuddles)
        {
            if (puddle != null)
            {
                Destroy(puddle);
            }
        }

        // Clear the list
        spawnedPuddles.Clear();
    }
}
