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
    protected float pieceBufferRatio = 0.2f;
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
    private int itemsMarkedForRemoval;
    private List<Transform> unfilledExits;
    private List<DungeonPiece> uncheckedDungeonPieces;
    private List<DungeonPiece> openConnectorPieces;
    private List<DungeonPiece> openIntersectionPieces;

    protected void Start()
    {
        InitDungeonBuilder();
        StartBuildingDungeon();
    }

    protected void InitDungeonBuilder()
    {
        spawnedPiecesCount = 0;
        itemsMarkedForRemoval = 0;
        unfilledExits = new List<Transform>();
        uncheckedDungeonPieces = new List<DungeonPiece>();
        openConnectorPieces = new List<DungeonPiece>();
        openIntersectionPieces = new List<DungeonPiece>();
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
        Debug.Log("Spawned pieces: " + spawnedPiecesCount);

        GatherUnusedExits();
        Debug.Log("Unchecked pieces: " + uncheckedDungeonPieces.Count + ". Unfilled exits: " + unfilledExits.Count);

        if (tryToConnectOpenExits)
        {
            ConnectUnusedExits();
        }

        Invoke("FinishDungeon", 0.5f);        
    }

    private void FinishDungeon()
    {
        if (cleanUpDeadEnds)
        {
            RemoveDeadEnds();
        }
        Debug.Log("Items removed: " + itemsMarkedForRemoval + ". Remaining exits: " + unfilledExits.Count);

        WallOffRemainingExits();
    }

    protected void GenerateDungeonPieces()
    {
        DungeonPiece firstDP = GetFirstDungeonPiece();
        Transform firstEntrance = firstDP.AttachToExit(initialEntrancePosition);
        firstDP.RemoveEntrance(firstEntrance);

        uncheckedDungeonPieces.Add(firstDP);

        while (uncheckedDungeonPieces.Count > 0 && spawnedPiecesCount < piecesToSpawn + (piecesToSpawn * pieceBufferRatio))
        {
            DungeonPiece pieceToCheck = uncheckedDungeonPieces[0];
            List<Transform> exitsToCheck = pieceToCheck.OpenExits;
            List<Transform> exitsToSkip = new List<Transform>();
            AddUnusedExits(pieceToCheck);

            Transform nextExit;         
            
            while(exitsToCheck.Count > 0 && exitsToSkip.Count != exitsToCheck.Count)
            {
                nextExit = exitsToCheck[0];

                GameObject instantiatedDP = AttemptToFillExit(nextExit, pieceToCheck);

                if (instantiatedDP != null)
                {
                    DungeonPiece dp = instantiatedDP.GetComponent<DungeonPiece>();
                    uncheckedDungeonPieces.Add(dp);
                    spawnedPiecesCount++;
                    unfilledExits.Remove(nextExit);
                }
                else
                {
                    if (!cleanUpDeadEnds)
                    {
                        FillExitWithWall(nextExit);
                        pieceToCheck.RemoveEntrance(nextExit);
                        unfilledExits.Remove(nextExit);
                    } else
                    {
                        exitsToSkip.Add(nextExit);
                    }
                }     
            }

            uncheckedDungeonPieces.RemoveAt(0);
        }
    }

    protected void GatherUnusedExits()
    {
        foreach (DungeonPiece uncheckedDP in uncheckedDungeonPieces)
        {
            AddUnusedExits(uncheckedDP);
        }

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
            default:
                break;
        }
    }

    protected void ConnectUnusedExits()
    {
        Debug.LogWarning("I'm sorry Dave I can't do that yet.");
    }

    protected void RemoveDeadEnds()
    {
        Debug.Log("OpenConnectors: " + openConnectorPieces.Count + ". OpenIntersections: " + openIntersectionPieces.Count);

        CleanUpOpenList(openConnectorPieces);

        CleanUpOpenList(openIntersectionPieces);

        Debug.Log("OpenConnectors: " + openConnectorPieces.Count + ". OpenIntersections: " + openIntersectionPieces.Count);

        Debug.Log("OpenConnectors: " + openConnectorPieces.Count + ". OpenIntersections: " + openIntersectionPieces.Count);
    }
    
    protected void CleanUpOpenList(List<DungeonPiece> listToClean)
    {
        while (listToClean.Count > 0)
        {
            DungeonPiece nextPieceToRemove = listToClean[0];

            while (nextPieceToRemove != null)
            {
                if (CanRemovePiece(nextPieceToRemove))
                {
                    DungeonPiece connectedPiece = (nextPieceToRemove.ExitPieceConnections.Count == 1) ?
                        nextPieceToRemove.ExitPieceConnections[0].connectedPiece : null;

                    if (listToClean.Contains(nextPieceToRemove)) { listToClean.Remove(nextPieceToRemove); }

                    RemoveDeadPiece(nextPieceToRemove);                    

                    nextPieceToRemove = connectedPiece;
                }
                else
                {
                    if (listToClean.Contains(nextPieceToRemove)) { listToClean.Remove(nextPieceToRemove); }

                    AddUnusedExits(nextPieceToRemove);

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
                if (dungeonPiece.Exits.Length <= 2 || dungeonPiece.ExitPieceConnections.Count == 1)//If it's not really an intersection
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

        DestroyDeadPiece(deadPiece);
    }

    protected void DestroyDeadPiece(DungeonPiece deadPiece)
    {    
        deadPiece.RemoveAllExitConnections();
        foreach(Transform exit in deadPiece.Exits)
        {
            if (unfilledExits.Contains(exit))
            {
                unfilledExits.Remove(exit);
            }
        }

        //MeshRenderer[] childRenderers = deadPiece.GetComponentsInChildren<MeshRenderer>();

        //foreach (MeshRenderer childRenderer in childRenderers)
        //{
        //    childRenderer.materials = new Material[0];
        //    childRenderer.material = null;
        //}
        Destroy(deadPiece.gameObject);

        itemsMarkedForRemoval++;
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
                    AddUnusedExits(lastRemovedPiece);
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
        GameObject parent = exit.GetComponent<ExitMarker>().ParentDungeonPiece.gameObject;
        if (parent != null)
        {           
            GameObject wallPiece = GameObject.Instantiate(dungeonPool.GetWallPiece(), parent.transform);

            wallPiece.transform.SetPositionAndRotation(exit.position, exit.rotation * Quaternion.Euler(0, 180, 0));
        }
        
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
