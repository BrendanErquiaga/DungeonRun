using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawnerWithDelay : ProceduralObjectSpawner
{
    public float initialDelay = 0f;
    public float spawnRate = 0.2f;

    private void Start()
    {
        StartSpawningObjects();
    }

    protected virtual void StartSpawningObjects()
    {
        if (!bagGenerator.objectBag.BagEmpty)
        {
            InvokeRepeating("SpawnObject", initialDelay, spawnRate);
        }
        else
        {
            Debug.LogWarning("You had no objects to spawn");
        }
    }

    protected override void SpawnObject()
    {
        base.SpawnObject();

        if (bagGenerator.objectBag.BagEmpty)
        {
            CancelInvoke("SpawnObject");
            Debug.Log("Done spawning!");
        }
    }
}
