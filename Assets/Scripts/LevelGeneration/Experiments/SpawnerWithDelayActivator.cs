using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithDelayActivator : SpawnerActivator
{
    public float spawnRate = 0.2f;

    public override void StartSpawningObjects()
    {
        if (!objectSpawner.BagGenerator.objectBag.BagEmpty)
        {
            InvokeRepeating("SpawnObject", 0, spawnRate);
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
        }
    }
}
