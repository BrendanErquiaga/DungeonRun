using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectBagGenerator : MonoBehaviour {

    [SerializeField]
    public ShuffleBag<GameObject> objectBag;

    [HideInInspector]
    public bool bagIsPrepared = false;

    [SerializeField]
    private int initialPiecesToGenerate = 100;    

    [SerializeField]
    private List<GameObjectWithInt> weightedObjects;

    private int sumOfChances;
    private int objectsInBag;

    private void Awake()
    {
        CalculateShuffleTable();
        GenerateShuffleBag();        
    }

    private void CalculateShuffleTable()
    {
        int tempSum = 0;

        foreach(GameObjectWithInt generatable in weightedObjects)
        {
            tempSum += generatable.value;
        }

        sumOfChances = tempSum;
    }

    private void GenerateShuffleBag()
    {
        List<GameObject> objectsForBag = new List<GameObject>();

        for (int i = 0; i < initialPiecesToGenerate; i++)
        {
            int r = Random.Range(1, sumOfChances + 1);
            GameObject objectToUse = null;

            int previousFloor = 0;

            foreach(GameObjectWithInt generatable in weightedObjects)
            {
                if(r <= generatable.value + previousFloor)
                {
                    objectToUse = generatable.objectToUse;
                    break;
                } else
                {
                    previousFloor += generatable.value;
                }
            }

            objectsForBag.Add(objectToUse);
            objectsInBag++;
        }

        objectBag = new ShuffleBag<GameObject>(objectsForBag, false);
        bagIsPrepared = true;
    }    
}
