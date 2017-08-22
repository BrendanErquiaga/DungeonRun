using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerSpawnGroupsConnector : MonoBehaviour
{
    [Tooltip("0 = All")]
    public int numberOfGroupsToAttach = 1;

    public void AttachSpawnGroupsToObjectSpawner(ProceduralObjectSpawner objectSpawner, SpawnGroupsHolder spawnGroupsHolder)
    {
        int shuffleTableSum = CalculateShuffleTable(spawnGroupsHolder);

        List<Transform> spawnLocations = GenerateSpawnGroups(spawnGroupsHolder, shuffleTableSum);

        objectSpawner.AddNewSpawnLocations(spawnLocations);
    }

    private int CalculateShuffleTable(SpawnGroupsHolder spawnGroupsHolder)
    {
        int tempSum = 0;

        foreach (SpawnGroup spawnGroup in spawnGroupsHolder.SpawnGroups)
        {
            tempSum += spawnGroup.SpawnGroupWeighting;
        }

        return tempSum;
    }

    private List<Transform> GenerateSpawnGroups(SpawnGroupsHolder spawnGroupsHolder, int shuffleTableSum)
    {
        List<Transform> spawnLocations = new List<Transform>();
        int numOfGroupsToAdd = numberOfGroupsToAttach;
        if (numberOfGroupsToAttach == 0)
        {
            numOfGroupsToAdd = spawnGroupsHolder.SpawnGroups.Count;
        }

        for (int i = 0; i < numOfGroupsToAdd; i++) 
        {
            int r = Random.Range(1, shuffleTableSum + 1);
            SpawnGroup groupToUse = null;

            int previousFloor = 0;

            foreach (SpawnGroup spawnGroup in spawnGroupsHolder.SpawnGroups)
            {
                if (r <= spawnGroup.SpawnGroupWeighting + previousFloor)
                {
                    groupToUse = spawnGroup;
                    break;
                }
                else
                {
                    previousFloor += spawnGroup.SpawnGroupWeighting;
                }
            }

            foreach(Transform spawnLocation in groupToUse.SpawnLocations)
            {
                spawnLocations.Add(spawnLocation);
            }
        }

        return spawnLocations;
    }
}
