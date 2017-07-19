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
    protected int piecePlacementRetries = 10;
    [SerializeField]
    protected bool tryToConnectOpenExits = false;//If false it will just wall them off
    [SerializeField]
    protected bool cleanUpDeadEnds = false;//If true remove connectors & intersections to nowhere
    [SerializeField]
    protected LayerMask layerMaskToCheck;

    private int spawnedPiecesCount;
    private List<Transform> unfilledExits;
    private DungeonPiece nextDungeonPiece;

    protected void Start()
    {
        InitDungeonBuilder();
        StartBuildingDungeon();
    }

    protected void InitDungeonBuilder()
    {
        spawnedPiecesCount = 0;
        unfilledExits = new List<Transform>();
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
        List<DungeonPiece> uncheckedDungeonPieces = new List<DungeonPiece>();

        DungeonPiece firstDP = GetFirstDungeonPiece();
        firstDP.AttachToExit(initialEntrancePosition);

        uncheckedDungeonPieces.Add(firstDP);
        
        while(uncheckedDungeonPieces.Count > 0 && spawnedPiecesCount < piecesToSpawn)
        {
            DungeonPiece pieceToCheck = uncheckedDungeonPieces[0];
            List<Transform> exitsToCheck = pieceToCheck.OpenExits;
            AddUnusedExits(pieceToCheck);    

            foreach(Transform exit in exitsToCheck)
            {
                GameObject instantiatedDP = AttemptToFillExit(exit, pieceToCheck);

                if(instantiatedDP != null)
                {
                    DungeonPiece dp = instantiatedDP.GetComponent<DungeonPiece>();
                    uncheckedDungeonPieces.Add(dp);
                    spawnedPiecesCount++;
                } else {
                    FillExitWithWall(exit);
                }

                unfilledExits.Remove(exit);
            }

            uncheckedDungeonPieces.RemoveAt(0);
        }

        foreach(DungeonPiece uncheckedDP in uncheckedDungeonPieces)
        {
            AddUnusedExits(uncheckedDP);
        }

        if(tryToConnectOpenExits)
        {
            //Connect them!
        } 

        if(cleanUpDeadEnds)
        {
            //Check for hallways to no-where, delete them
        }

        WallOffRemainingExits();
    }

    protected void WallOffRemainingExits()
    {
        foreach(Transform openExit in unfilledExits)
        {
            FillExitWithWall(openExit);
        }

        unfilledExits.Clear();
    }

    protected void FillExitWithWall(Transform exit)
    {
        GameObject wallPiece = InstantiateChildGameObject(dungeonPool.GetWallPiece());

        wallPiece.transform.SetPositionAndRotation(exit.position, exit.rotation * Quaternion.Euler(0, 180, 0));
    }

    protected GameObject AttemptToFillExit(Transform exitToFill, DungeonPiece connectedPiece)
    {
        int placementAttempts = 0;
        GameObject placedObject = null;
        DungeonPiece potentialDungeonPiece = null;
        bool pieceConnectedSafely = false;

        while(!pieceConnectedSafely && placementAttempts < piecePlacementRetries)
        {
            if(placedObject != null) { Destroy(placedObject); }//Clean up any unused pieces
            placedObject = InstantiateChildGameObject(GetNextDungeonPiece(connectedPiece));
            potentialDungeonPiece = placedObject.GetComponent<DungeonPiece>();
            placementAttempts++;
            pieceConnectedSafely = PieceConnectedSafely(potentialDungeonPiece, exitToFill);
        } 

        if(placementAttempts >= piecePlacementRetries)
        {
            Destroy(placedObject);
            placedObject = null;
        }

        return placedObject;
    }

    protected DungeonPiece GetFirstDungeonPiece()
    {
        GameObject instantiatedDungeonPiece = InstantiateChildGameObject(dungeonPool.GetDungeonPiece());   

        return instantiatedDungeonPiece.GetComponent<DungeonPiece>();
    }

    protected bool PieceConnectedSafely(DungeonPiece dungeonPiece, Transform exit)
    {
        bool objectSafelyPlaced = true;
        Transform entranceUsed = dungeonPiece.AttachToExit(exit);

        Bounds nextObjectBounds = Extensions.GetBoundsOfChildren(dungeonPiece.gameObject);
        Extensions.ToggleChildColliders(dungeonPiece.gameObject, false);
        if (Physics.CheckBox(nextObjectBounds.center, nextObjectBounds.extents * 0.9f, Quaternion.identity, layerMaskToCheck))
        {
            objectSafelyPlaced = false;
        } else
        {
            dungeonPiece.RemoveEntrance(entranceUsed);//Make sure we don't leave this listed as an open Entrance
            Extensions.ToggleChildColliders(dungeonPiece.gameObject, true);
        }

        return objectSafelyPlaced;
    }

    protected void AddUnusedExits(DungeonPiece dungeonPiece)
    {
        for(int i = 0; i < dungeonPiece.OpenExits.Count; i++)
        {
            unfilledExits.Add(dungeonPiece.OpenExits[i]);
        }
    }

    protected GameObject GetNextDungeonPiece(DungeonPiece connectedPiece)
    {
        return dungeonPool.GetDungeonPiece(connectedPiece);   
    }

    protected GameObject InstantiateChildGameObject(GameObject prefab)
    {
        return GameObject.Instantiate(prefab, this.transform);
    }
}
