using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour
{
    public ProceduralObjectBagGenerator bagGenerator;
    
    public bool addForceToObjectsOnSpawn = false;
    public float randomForce = 10f;

    [SerializeField]
    protected List<Transform> spawnLocations;

    protected GameObject previouslySpawnedObject;
    private int spawnedItemCount = 0;
    private ShuffleBag<Transform> spawnLocationShufflebag;

    private void Awake()
    {
        if(spawnLocations.Count > 0) {
            spawnLocationShufflebag = new ShuffleBag<Transform>(spawnLocations, true);
        } else
        {
            spawnLocationShufflebag = new ShuffleBag<Transform>(new List<Transform>());
        }        
    }

    protected virtual void SpawnObject()
    {
        if (bagGenerator.objectBag.BagEmpty)
        {
            Debug.Log("Bag is empty, you can't spawn...");
            return;
        }

        Vector3 spawnLocation = GetSpawnLocation();

        GameObject objectToInstantiate = bagGenerator.objectBag.GetNextItemInBag();

        previouslySpawnedObject = GameObject.Instantiate(objectToInstantiate, spawnLocation, objectToInstantiate.transform.rotation, this.transform);

        if(addForceToObjectsOnSpawn)
        {
            AddForceToObject(previouslySpawnedObject);
        }

        spawnedItemCount++;
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
}
