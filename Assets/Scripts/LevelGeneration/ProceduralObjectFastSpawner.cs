using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectFastSpawner : ProceduralObjectSpawner
{
    [SerializeField]
    private float initialDelay = 0;

    private void Start()
    {
        Invoke("StartSpawningObjects", initialDelay);
    }

    protected virtual void StartSpawningObjects()
    {
        while (!bagGenerator.objectBag.BagEmpty)
        {
            SpawnObject();
        }
    }
}
