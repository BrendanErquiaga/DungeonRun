using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour
{
    public List<Transform> spawnLocations;
    private ShuffleBag<Transform> spawnLocationShufflebag;

    public ProceduralObjectBagGenerator bagGenerator;
    
    public bool addForceToObjectsOnSpawn = false;
    public float randomForce = 10f;

    private int spawnedItemCount = 0;

    private void Awake()
    {
        spawnLocationShufflebag = new ShuffleBag<Transform>(spawnLocations, true);
    }

    protected virtual void SpawnObject()
    {
        if (bagGenerator.objectBag.BagEmpty)
        {
            Debug.Log("Bag is empty, you can't spawn...");
            return;
        }

        Transform spawnLocation = GetSpawnLocation();

        GameObject objectToInstantiate = bagGenerator.objectBag.GetNextItemInBag();

        GameObject instantiatedObject = GameObject.Instantiate(objectToInstantiate, spawnLocation.position, objectToInstantiate.transform.rotation, this.transform);

        if(addForceToObjectsOnSpawn)
        {
            AddForceToObject(instantiatedObject);
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

    protected virtual Transform GetSpawnLocation()
    {
        if(spawnLocations.Count == 0 && spawnLocationShufflebag.BagEmpty)
        {
            return this.transform;
        } else
        {
            return spawnLocationShufflebag.GetNextItemInBag();
        }
    }
}
