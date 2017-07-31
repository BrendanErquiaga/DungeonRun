using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFloodFetcher : DungeonPieceFetcher
{
    [SerializeField]
    protected bool allowFetchedPiecesIntoListOnReorder = false;

    protected LinkedList<DungeonPiece> uncheckedCenterPieces;

    protected override void BuildUnfetchedList()
    {
        this.unfetchedPieces = new LinkedList<DungeonPiece>();
        
        int r = Random.Range(0, this.pieceTracker.SpawnedDungeonPieces.Count);
        DungeonPiece seedPiece = this.pieceTracker.SpawnedDungeonPieces[r];

        BuildlistFromSeed(seedPiece, this.pieceTracker.SpawnedDungeonPieces);
    }

    public override void ReorderListWithNewSeed()
    {
        int r = Random.Range(0, this.unfetchedPieces.Count);
        List<DungeonPiece> newOrderUnfetchedPieces = new List<DungeonPiece>(this.unfetchedPieces);

        BuildlistFromSeed(newOrderUnfetchedPieces[r], newOrderUnfetchedPieces);
    }

    protected virtual void BuildlistFromSeed(DungeonPiece seedPiece, List<DungeonPiece> seedList)
    {
        this.uncheckedCenterPieces = new LinkedList<DungeonPiece>();
        this.uncheckedCenterPieces.AddLast(seedPiece);

        while (this.uncheckedCenterPieces.Count > 0)
        {
            List<DungeonPiece> connectedPieces = GetConnectedPieces(GetNextCenterPiece());
            AddConnectedPieces(connectedPieces);
        }
    }

    protected virtual DungeonPiece GetNextCenterPiece()
    {
        if (this.uncheckedCenterPieces.Count == 0) {
            return null;
        }

        DungeonPiece nextCenterPiece = this.uncheckedCenterPieces.First.Value;

        this.uncheckedCenterPieces.Remove(nextCenterPiece);

        return nextCenterPiece;
    }

    protected virtual void AddConnectedPieces(List<DungeonPiece> connectedPieces)
    {
        foreach (DungeonPiece connectedPiece in connectedPieces)
        {
            if(AllowedToAddPiece(connectedPiece))
            {
                this.unfetchedPieces.AddLast(connectedPiece);

                if (!this.uncheckedCenterPieces.Contains(connectedPiece))
                {
                    this.uncheckedCenterPieces.AddLast(connectedPiece);
                }
            }                                 
        }
    }

    protected virtual bool AllowedToAddPiece(DungeonPiece dungeonPiece)
    {
        bool pieceAllowed = false;

        if (!this.unfetchedPieces.Contains(dungeonPiece))
        {
            if(allowFetchedPiecesIntoListOnReorder || !this.fetchedPieces.Contains(dungeonPiece))
            {
                pieceAllowed = true;
            }
        }

        return pieceAllowed;
    }

    protected List<DungeonPiece> GetConnectedPieces(DungeonPiece centralPiece)
    {
        List<DungeonPiece> connectedPieces = new List<DungeonPiece>();

        if(centralPiece == null)
        {
            return connectedPieces;
        }

        foreach (PieceConnection connection in centralPiece.ExitPieceConnections)
        {
            connectedPieces.Add(connection.connectedPiece);
        }

        return connectedPieces;
    }
}
