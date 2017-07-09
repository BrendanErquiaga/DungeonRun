using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSpawnerActivator : MonoBehaviour
{
    [SerializeField]
    private float initialDelay = 0;
    [SerializeField]
    protected ProceduralObjectSpawner objectSpawner;

    private void Start()
    {
        Invoke("StartSpawningObjects", initialDelay);
    }

    protected virtual void StartSpawningObjects()
    {
        while (!objectSpawner.BagGenerator.objectBag.BagEmpty)
        {
            objectSpawner.SpawnObject();
        }
    }
}

