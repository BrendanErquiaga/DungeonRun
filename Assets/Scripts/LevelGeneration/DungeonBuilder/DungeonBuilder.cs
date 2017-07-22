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
    private List<DungeonPiece> uncheckedDungeonPieces;
    private List<DungeonPiece> openConnectorPieces;
    private List<DungeonPiece> openIntersectionPieces;
    private List<DungeonPiece> openRoomPieces;
    //private DungeonPiece nextDungeonPiece;

    protected void Start()
    {
        InitDungeonBuilder();
        StartBuildingDungeon();
    }

    protected void InitDungeonBuilder()
    {
        spawnedPiecesCount = 0;
        unfilledExits = new List<Transform>();
        uncheckedDungeonPieces = new List<DungeonPiece>();
        openConnectorPieces = new List<DungeonPiece>();
        openIntersectionPieces = new List<DungeonPiece>();
        openRoomPieces = new List<DungeonPiece>();
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
        GenerateDungeonPieces();

        GatherUnusedExits();

        if(tryToConnectOpenExits)
        {
            ConnectUnusedExits();
        } 

        if(cleanUpDeadEnds)
        {
            RemoveDeadEnds();
        }

        //WallOffRemainingExits();
    }

    protected void GenerateDungeonPieces()
    {
        DungeonPiece firstDP = GetFirstDungeonPiece();
        firstDP.AttachToExit(initialEntrancePosition);

        uncheckedDungeonPieces.Add(firstDP);

        while (uncheckedDungeonPieces.Count > 0 && spawnedPiecesCount < piecesToSpawn)
        {
            DungeonPiece pieceToCheck = uncheckedDungeonPieces[0];
            List<Transform> exitsToCheck = pieceToCheck.OpenExits;
            AddUnusedExits(pieceToCheck);

            foreach (Transform exit in exitsToCheck)
            {
                GameObject instantiatedDP = AttemptToFillExit(exit, pieceToCheck);

                if (instantiatedDP != null)
                {
                    DungeonPiece dp = instantiatedDP.GetComponent<DungeonPiece>();
                    uncheckedDungeonPieces.Add(dp);
                    spawnedPiecesCount++;
                }
                else
                {
                    if (!cleanUpDeadEnds) {
                        FillExitWithWall(exit);
                    }                    
                }

                unfilledExits.Remove(exit);
            }

            uncheckedDungeonPieces.RemoveAt(0);
        }
    }

    protected void GatherUnusedExits()
    {
        Debug.Log("Unchecked pieces: " + uncheckedDungeonPieces.Count + ". Unfilled exits: " + unfilledExits.Count);
        foreach (DungeonPiece uncheckedDP in uncheckedDungeonPieces)
        {
            AddUnusedExits(uncheckedDP);
        }

        Debug.Log("Unchecked pieces: " + uncheckedDungeonPieces.Count + ". Unfilled exits: " + unfilledExits.Count);
        foreach (Transform exit in unfilledExits)
        {
            DungeonPiece parentPiece = exit.GetComponent<ExitMarker>().ParentDungeonPiece;

            SeperateUnfilledDungeonPieces(parentPiece);
        }
    }

    protected void SeperateUnfilledDungeonPieces(DungeonPiece dungeonPiece)
    {
        switch(dungeonPiece.PieceType)
        {
            case DungeonPieceType.CONNECTOR:
                if (!openConnectorPieces.Contains(dungeonPiece))
                {
                    openConnectorPieces.Add(dungeonPiece);
                }
                break;
            case DungeonPieceType.INTERSECTION:
                if (!openIntersectionPieces.Contains(dungeonPiece))
                {
                    openIntersectionPieces.Add(dungeonPiece);
                }
                break;
            case DungeonPieceType.ROOM:
                if (!openRoomPieces.Contains(dungeonPiece))
                {
                    openRoomPieces.Add(dungeonPiece);
                }
                break;
        }
    }

    protected void ConnectUnusedExits()
    {

    }

    protected void RemoveDeadEnds()
    {
        Debug.Log("Cleanup time, Open Connectors: " + openConnectorPieces.Count);
        Debug.Log("Cleanup time, Open Intersections: " + openIntersectionPieces.Count);
        CleanUpOpenList(openConnectorPieces);

        Debug.Log("Connections cleaned, Open Connectors: " + openConnectorPieces.Count);
        Debug.Log("Connections cleaned, Open Intersections: " + openIntersectionPieces.Count);
        CleanUpOpenList(openIntersectionPieces);

        Debug.Log("Intersections cleaned, Open Connectors: " + openConnectorPieces.Count);
        Debug.Log("Intersections cleaned, Open Intersections: " + openIntersectionPieces.Count);
    }

    protected void CleanUpOpenList(List<DungeonPiece> listToClean)
    {
        Debug.Log("Starting to clean list of size: " + listToClean.Count);
        while (listToClean.Count > 0)
        {
            DungeonPiece nextPieceToRemove = listToClean[0];

            while (nextPieceToRemove != null)
            {
                if (CanRemovePiece(nextPieceToRemove))
                {
                    DungeonPiece connectedPiece = (nextPieceToRemove.ExitPieceConnections.Count == 1) ?
                        nextPieceToRemove.ExitPieceConnections[0].connectedPiece : null;

                    RemoveDeadPiece(nextPieceToRemove);

                    if(listToClean.Contains(nextPieceToRemove)) { listToClean.Remove(nextPieceToRemove); }

                    nextPieceToRemove = connectedPiece;
                }
                else
                {
                    Debug.Log("I couldn't remove this object...", nextPieceToRemove);
                    if (listToClean.Contains(nextPieceToRemove)) { listToClean.Remove(nextPieceToRemove); }
                    nextPieceToRemove = null;
                }
            }
        }
    }

    protected bool CanRemovePiece(DungeonPiece dungeonPiece)
    {
        bool canRemovePiece = false;

        switch (dungeonPiece.PieceType)
        {
            case DungeonPieceType.CONNECTOR:
                canRemovePiece = true;
                break;
            case DungeonPieceType.INTERSECTION:
                if (dungeonPiece.Exits.Length <= 2)//If it's not really an intersection
                {
                    canRemovePiece = true;
                }
                break;
        }

        return canRemovePiece;
    }

    protected void RemoveDeadPiece(DungeonPiece deadPiece)
    {
        if(openConnectorPieces.Contains(deadPiece)) { openConnectorPieces.Remove(deadPiece); }
        if(openIntersectionPieces.Contains(deadPiece)) { openIntersectionPieces.Remove(deadPiece); }
        //if(openRoomPieces.Contains(deadPiece)) { openRoomPieces.Remove(deadPiece); }

        DestroyDeadPiece(deadPiece);
    }

    protected void DestroyDeadPiece(DungeonPiece deadPiece)
    {
        MeshRenderer[] childRenderers = deadPiece.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer childRenderer in childRenderers)
        {
            childRenderer.materials = new Material[0];
            childRenderer.material = null;
        }       

        deadPiece.RemoveAllExitConnections();
        foreach(Transform exit in deadPiece.OpenExits)
        {
            if (unfilledExits.Contains(exit))
            {
                unfilledExits.Remove(exit);
            }
        }
    }

    protected List<DungeonPiece> GetAllRemovablePieces(DungeonPiece deadEndStart)
    {
        List<DungeonPiece> piecesToRemove = new List<DungeonPiece>();

        bool stillRemovingPieces = false;
        DungeonPiece lastRemovedPiece = deadEndStart;

        do
        {
            foreach(PieceConnection piece in lastRemovedPiece.ExitPieceConnections)
            {
                lastRemovedPiece = piece.connectedPiece;
                if (CanRemovePiece(lastRemovedPiece))
                {
                    piecesToRemove.Add(lastRemovedPiece);
                }
                else
                {
                    stillRemovingPieces = false;
                }
            }
            
        } while (stillRemovingPieces);

        return piecesToRemove;
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
            dungeonPiece.ConnectPieceToExit(entranceUsed, exit);//Make sure we don't leave this listed as an open Entrance
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
