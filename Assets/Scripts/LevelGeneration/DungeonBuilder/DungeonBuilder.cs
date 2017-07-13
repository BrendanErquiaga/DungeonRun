using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField]
    protected DungeonPiecePool dungeonPool;
    [SerializeField]
    protected int piecesToSpawn = 10;
    [SerializeField]
    protected Transform initialEntrancePosition;
    [SerializeField]
    protected LayerMask layerMaskToCheck;

    private int spawnedPieces;
    private List<Transform> exitsToFill;
    private DungeonPiece nextDungeonPiece;
    private DungeonPiece lastDungeonPiece;

    protected void Start()
    {
        InitDungeonBuilder();
        StartBuildingDungeon();
    }

    protected void InitDungeonBuilder()
    {
        spawnedPieces = 0;
        exitsToFill = new List<Transform>();
        exitsToFill.Add(initialEntrancePosition);
    }

    protected void StartBuildingDungeon()
    {
        if (dungeonPool.poolIsPrepared)
        {
            BuildDungeon();
        } else
        {
            Invoke("StartBuildingDungeon", 0.1f);
        }
    }

    protected void BuildDungeon()
    {
        while (exitsToFill.Count > 0 && spawnedPieces < piecesToSpawn)
        {
            GameObject instantiatedDungeonPiece = InstantiateDungeonPiece(GetNextDungeonPiece());
            DungeonPiece potentialDungeonPiece = instantiatedDungeonPiece.GetComponent<DungeonPiece>();

            if (PieceConnectedSafely(potentialDungeonPiece, exitsToFill[0]))
            {
                nextDungeonPiece = potentialDungeonPiece;
                lastDungeonPiece = nextDungeonPiece;
                AddUnusedExits(nextDungeonPiece);
                spawnedPieces++;
            } else
            {
                Destroy(instantiatedDungeonPiece);
            }

            exitsToFill.RemoveAt(0);            
        } 
    }

    protected bool PieceConnectedSafely(DungeonPiece dungeonPiece, Transform exit)
    {
        bool objectSafelyPlaced = true;
        dungeonPiece.AttachToExit(exit);

        Bounds nextObjectBounds = Extensions.GetBoundsOfChildren(dungeonPiece.gameObject);
        //dungeonPiece.gameObject.SetActive(false);
        Extensions.ToggleChildColliders(dungeonPiece.gameObject, false);
        if (Physics.CheckBox(nextObjectBounds.center, nextObjectBounds.extents * 0.9f, Quaternion.identity, layerMaskToCheck))
        {
            Debug.Log("Well this shouldn't go there: " + dungeonPiece.gameObject.name);
            objectSafelyPlaced = false;
        } else
        {
            //dungeonPiece.gameObject.SetActive(true);
            Extensions.ToggleChildColliders(dungeonPiece.gameObject, true);
        }

        return objectSafelyPlaced;
    }

    protected void AddUnusedExits(DungeonPiece dungeonPiece)
    {
        for(int i = 0; i <= dungeonPiece.OpenExits.Count; i++)
        {
            exitsToFill.Add(dungeonPiece.GetNextOpenExit());
            //Debug.Log("Added exit: " + exitsToFill[exitsToFill.Count - 1] + " .Time: " 
            //    + Time.deltaTime + ". for: " + dungeonPiece.gameObject.name);
        }
    }

    protected GameObject GetNextDungeonPiece()
    {
        if(lastDungeonPiece != null)
        {
            return dungeonPool.GetDungeonPiece(lastDungeonPiece);
        } else
        {
            return dungeonPool.GetDungeonPiece();
        }        
    }

    protected GameObject InstantiateDungeonPiece(GameObject prefab)
    {
        return GameObject.Instantiate(prefab, this.transform);
    }
}
