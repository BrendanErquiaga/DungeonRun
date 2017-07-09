using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithDelayActivator : MonoBehaviour
{
    public float initialDelay = 0f;
    public float spawnRate = 0.2f;

    [SerializeField]
    protected ProceduralObjectSpawner objectSpawner;

    private void Start()
    {
        StartSpawningObjects();
    }

    protected virtual void StartSpawningObjects()
    {
        if (!objectSpawner.BagGenerator.objectBag.BagEmpty)
        {
            InvokeRepeating("SpawnObject", initialDelay, spawnRate);
        }
        else
        {
            Debug.LogWarning("You had no objects to spawn");
        }
    }

    protected virtual void SpawnObject()
    {
        objectSpawner.SpawnObject();

        if (objectSpawner.BagGenerator.objectBag.BagEmpty)
        {
            CancelInvoke("SpawnObject");
            Debug.Log("Done spawning!");
        }
    }
}
