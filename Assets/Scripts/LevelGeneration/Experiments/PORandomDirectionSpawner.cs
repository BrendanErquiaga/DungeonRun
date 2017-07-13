using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PORandomDirectionSpawner : ProceduralObjectRowSpawner
{
    protected Vector3 previousSpawnDirection;

    [SerializeField]
    protected bool getRandomDirectionAfterFindingCollision = false;

    [SerializeField]
    protected List<Vector3> randomDirections = new List<Vector3> { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    protected override void Initialize()
    {
        base.Initialize();
        spawnDirection = GetRandomSpawnDirection();
    }

    public override void SpawnObject()
    {
        base.SpawnObject();
        previousSpawnDirection = spawnDirection;
    }

    protected override Vector3 GetSpawnLocation()
    {
        if(previousSpawnDirection != null)
        {
            spawnDirection = GetRandomSpawnDirection();        
        }

        return base.GetSpawnLocation();
    }

    protected virtual Vector3 GetRandomSpawnDirection()
    {
        return randomDirections[Random.Range(0, randomDirections.Count)];        
    }

    protected override Vector3 GetUniformRelocatedSpawnLocation(Vector3 possibleSpawnLocation)
    {
        Vector3 nextObjectBounds = nextObjectToSpawn.GetComponent<Collider>().bounds.size;

        Vector3 raySpawnPostion = new Vector3(possibleSpawnLocation.x,
            possibleSpawnLocation.y + nextObjectBounds.y + 1,
            possibleSpawnLocation.z);

        if (Physics.Raycast(raySpawnPostion, -Vector3.up, raySpawnPostion.y - possibleSpawnLocation.y, layerMaskToCheck))
        {
            previousSpawnLocation = possibleSpawnLocation;
            if(getRandomDirectionAfterFindingCollision)
                return GetUniformRelocatedSpawnLocation(GetSpawnLocation());
            else
                return GetUniformRelocatedSpawnLocation(base.GetSpawnLocation());
        }

        return possibleSpawnLocation;   
    }

    protected override Vector3 GetRelocatedSpawnPosition(Vector3 possibleSpawnLocation)
    {
        Vector3 nextObjectBounds = nextObjectToSpawn.GetComponent<Collider>().bounds.size;

        if(Physics.CheckBox(possibleSpawnLocation, nextObjectBounds, nextObjectToSpawn.transform.rotation, layerMaskToCheck))
        {
            previousSpawnLocation = possibleSpawnLocation;
            if (getRandomDirectionAfterFindingCollision)
                return GetRelocatedSpawnPosition(GetSpawnLocation());
            else
                return GetRelocatedSpawnPosition(base.GetSpawnLocation());
        }

        return possibleSpawnLocation;
    }
}
