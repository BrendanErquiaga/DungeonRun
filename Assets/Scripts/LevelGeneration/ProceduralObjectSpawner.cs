using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private ProceduralObjectBagGenerator bagGenerator;
    [SerializeField]
    protected bool addForceToObjectsOnSpawn = false;
    [SerializeField]
    protected float randomForce = 10f;

    [SerializeField]
    protected SpawnerCreationBehavior creationBehavior = SpawnerCreationBehavior.Overlap;
    [SerializeField]
    protected LayerMask layerMaskToCheck;

    [SerializeField]
    protected List<Transform> spawnLocations;

    protected GameObject nextObjectToSpawn;
    protected GameObject previouslySpawnedObject;
    protected Vector3 previousSpawnLocation;
    private int spawnedItemCount = 0;
    private ShuffleBag<Transform> spawnLocationShufflebag;

    public ProceduralObjectBagGenerator BagGenerator
    {
        get
        {
            return bagGenerator;
        }

        protected set
        {
            bagGenerator = value;
        }
    }

    private void Awake()
    {
        Initialize();  
    }

    protected virtual void Initialize()
    {
        if (spawnLocations.Count > 0)
        {
            spawnLocationShufflebag = new ShuffleBag<Transform>(spawnLocations, true);
        }
        else
        {
            spawnLocationShufflebag = new ShuffleBag<Transform>(new List<Transform>());
        }
    }

    public virtual void SpawnObject()
    {
        if (BagGenerator.objectBag.BagEmpty)
        {
            Debug.Log("Bag is empty, you can't spawn...");
            return;
        }

        nextObjectToSpawn = GetNextObject();
        Vector3 spawnLocation = GetSpawnLocation();
        spawnLocation = GetAcceptableSpawnPosition(spawnLocation);

        previouslySpawnedObject = GameObject.Instantiate(nextObjectToSpawn, spawnLocation, nextObjectToSpawn.transform.rotation, this.transform);
        previouslySpawnedObject.name = previouslySpawnedObject.name + "[" + spawnedItemCount + "]";

        if(addForceToObjectsOnSpawn)
        {
            AddForceToObject(previouslySpawnedObject);
        }

        spawnedItemCount++;

        previousSpawnLocation = spawnLocation;
    }

    protected GameObject GetNextObject()
    {
        return BagGenerator.objectBag.GetNextItemInBag();
    }

    protected virtual void AddForceToObject(GameObject objectToAddForceTo)
    {
        Rigidbody instantiatedRigidBody = objectToAddForceTo.GetComponent<Rigidbody>();

        instantiatedRigidBody.AddForce(GetRandomForce());
    }

    protected virtual Vector3 GetRandomForce()
    {
        float x = Random.Range(0, randomForce);
        float y = Random.Range(0, randomForce);
        float z = Random.Range(0, randomForce);

        return new Vector3(x, y, z);
    }

    protected virtual Vector3 GetSpawnLocation()
    {
        if(spawnLocations.Count == 0 && spawnLocationShufflebag.BagEmpty)
        {
            return this.transform.position;
        } else
        {
            return spawnLocationShufflebag.GetNextItemInBag().position;
        }
    }

    protected virtual Vector3 GetAcceptableSpawnPosition(Vector3 possibleSpawnPosition)
    {
        Vector3 acceptablePosition = possibleSpawnPosition;
        switch (creationBehavior)
        {
            case SpawnerCreationBehavior.RaycastRelocate:
                acceptablePosition = GetUniformRelocatedSpawnLocation(possibleSpawnPosition);
                break;
            case SpawnerCreationBehavior.BoxCastRelocate:
                acceptablePosition = GetRelocatedSpawnPosition(possibleSpawnPosition);
                break;
            default:
                break;
        }

        return acceptablePosition;
    }

    protected virtual Vector3 GetUniformRelocatedSpawnLocation(Vector3 possibleSpawnLocation)
    {
        return possibleSpawnLocation;
    }

    protected virtual Vector3 GetRelocatedSpawnPosition(Vector3 possibleSpawnLocation)
    {
        return possibleSpawnLocation;
    }
}

public enum SpawnerCreationBehavior
{
    Overlap,
    RaycastRelocate,
    BoxCastRelocate
}
