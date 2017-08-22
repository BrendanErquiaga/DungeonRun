using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerHandler : AnyDungeonPieceBasePropertyHandler
{
    [SerializeField]
    private PropertyGameObjectMap objectSpawnerMap;

    [SerializeField]
    private ObjectSpawnerSpawnGroupsConnector objectSpawnerSpawnGroupsConnector;

    protected override void HandlePropertyAdded(DungeonPiece dungeonPiece, string propertyKey, string property)
    {
        base.HandlePropertyAdded(dungeonPiece, propertyKey, property);

        if(PropertyKeysMatch(propertyKey) 
            && dungeonPiece.PieceType == DungeonPieceType.ROOM 
            && objectSpawnerMap.ContainsKey(property))
        {
            GameObject spawnerCopy = Instantiate(objectSpawnerMap[property], dungeonPiece.transform);
            
            spawnerCopy.SetActive(true);
            ProceduralObjectSpawner objectSpawner = spawnerCopy.GetComponent<ProceduralObjectSpawner>();
            SpawnGroupsHolder spawnGroups = dungeonPiece.GetComponent<SpawnGroupsHolder>();
            SpawnerActivator activator = spawnerCopy.GetComponent<SpawnerActivator>();

            objectSpawnerSpawnGroupsConnector.AttachSpawnGroupsToObjectSpawner(objectSpawner, spawnGroups);

            activator.StartSpawningObjects();
        }
    }
}
