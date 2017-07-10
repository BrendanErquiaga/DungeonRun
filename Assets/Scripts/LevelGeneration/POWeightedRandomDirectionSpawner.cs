using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POWeightedRandomDirectionSpawner : PORandomDirectionSpawner
{
    [SerializeField]
    private List<Vector3WithInt> weightedDirections;

    protected override void Initialize()
    {
        GenerateRandomDirectionBag();

        base.Initialize();        
    }

    protected virtual void GenerateRandomDirectionBag()
    {
        randomDirections.Clear();
        for(int i = 0; i < weightedDirections.Count; i++)
        {
            for(int j = 0; j < weightedDirections[i].value; j++)
            {
                randomDirections.Add(weightedDirections[i].vectorToUse);
            }
        }
    }
}
