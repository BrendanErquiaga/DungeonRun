using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSpawnerActivator : SpawnerActivator
{
    public override void StartSpawningObjects()
    {
        while (!objectSpawner.BagGenerator.objectBag.BagEmpty)
        {
            objectSpawner.SpawnObject();
        }
    }
}

