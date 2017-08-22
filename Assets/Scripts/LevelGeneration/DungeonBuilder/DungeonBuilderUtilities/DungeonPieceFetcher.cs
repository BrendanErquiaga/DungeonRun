using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPieceFetcher : MonoBehaviour
{
    [SerializeField]
    protected DungeonPieceTracker pieceTracker;

    protected List<DungeonPiece> fetchedPieces;
    protected LinkedList<DungeonPiece> unfetchedPieces;
    protected bool fetcherInitialized = false;

    public bool CanFetch
    {
        get
        {
            if (!fetcherInitialized)
            {
                InitFetcher();
            }

            if(fetcherInitialized && unfetchedPieces.Count >= 1)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

    public DungeonPiece FetchNextObject()
    {
        if (!this.fetcherInitialized)
        {
            InitFetcher();
        }

        if(!this.CanFetch)
        {
            return null;
        }

        DungeonPiece pieceToFetch = FetchNextObjectActual();

        unfetchedPieces.Remove(pieceToFetch);
        fetchedPieces.Add(pieceToFetch);

        return pieceToFetch;
    }

    public virtual void ReorderListWithNewSeed()
    {
        this.BuildUnfetchedList(new List<DungeonPiece>(this.unfetchedPieces));
    }

    protected virtual DungeonPiece FetchNextObjectActual()
    {       
        return unfetchedPieces.First.Value;
    }

    protected virtual void InitFetcher()
    {
        fetchedPieces = new List<DungeonPiece>();
        unfetchedPieces = new LinkedList<DungeonPiece>();
        BuildUnfetchedList();

        fetcherInitialized = true;
    }

    protected virtual void BuildUnfetchedList()
    {
        BuildUnfetchedList(this.pieceTracker.SpawnedDungeonPieces);
    }

    protected virtual void BuildUnfetchedList(List<DungeonPiece> listToBuildFrom)
    {
        foreach (DungeonPiece dungeonPiece in listToBuildFrom)
        {
            if(dungeonPiece != null)
            {
                unfetchedPieces.AddFirst(dungeonPiece);
            } else
            {
                Debug.LogError("Can't add a piece that doesn't exist...");
            }
            
        }
    }
}

