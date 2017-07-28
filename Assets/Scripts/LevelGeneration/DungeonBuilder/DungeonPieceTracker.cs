using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPieceTracker : MonoBehaviour
{
    private List<DungeonPiece> spawnedDungeonPieces;

    public List<DungeonPiece> SpawnedDungeonPieces
    {
        get
        {
            return spawnedDungeonPieces;
        }

        private set
        {
            spawnedDungeonPieces = value;
        }
    }

    void Start ()
    {
        SpawnedDungeonPieces = new List<DungeonPiece>();

        DungeonPiece.DungeonPieceSpawned += DungeonPiece_DungeonPieceSpawned;
        DungeonPiece.DungeonPieceDestroyed += DungeonPiece_DungeonPieceDestroyed;
	}

    private void OnDestroy()
    {
        DungeonPiece.DungeonPieceSpawned -= DungeonPiece_DungeonPieceSpawned;
        DungeonPiece.DungeonPieceDestroyed -= DungeonPiece_DungeonPieceDestroyed;
    }

    private void DungeonPiece_DungeonPieceSpawned(DungeonPiece dungeonPiece)
    {
        SpawnedDungeonPieces.Add(dungeonPiece);
    }

    private void DungeonPiece_DungeonPieceDestroyed(DungeonPiece dungeonPiece)
    {
        SpawnedDungeonPieces.Remove(dungeonPiece);
    }
}
