using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectRowSpawner : ProceduralObjectSpawner
{
    [SerializeField]
    protected Vector3 spawnDirection = Vector3.forward;

    protected override Vector3 GetSpawnLocation()
    {
        if(previouslySpawnedObject == null)
        {
            return base.GetSpawnLocation();
        } else
        {
            Vector3 newSpawnLocation = previousSpawnLocation;

            Vector3 boundsOfPreviousObject = previouslySpawnedObject.GetComponent<Renderer>().bounds.size;

            Vector3 distance = new Vector3(boundsOfPreviousObject.x * spawnDirection.x,
                boundsOfPreviousObject.y * spawnDirection.y, 
                boundsOfPreviousObject.z * spawnDirection.z);

            newSpawnLocation = new Vector3(newSpawnLocation.x + distance.x,
                                    newSpawnLocation.y + distance.y,
                                    newSpawnLocation.z + distance.z);

            return newSpawnLocation;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawLine(transform.position, transform.position + (spawnDirection * 10));
    }
}
