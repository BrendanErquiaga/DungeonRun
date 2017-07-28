using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiecePropertyGenerator : MonoBehaviour
{
    [SerializeField]
    protected DungeonPieceTracker dungeonTracker;
    [SerializeField]
    protected PropertyPool propertyPool;
    [SerializeField]
    protected string seedProperty = "";
    [SerializeField]
    protected float chanceOfRepeatProperty = 0.9f;
    [SerializeField]
    protected float repeatPropertyDecayRate = 1;//Applies to region chance of repeat after each spawn
    [SerializeField]
    private string propertyKey;

    protected void Start()
    {
        InitRegionGenerator();
    }

    protected virtual void InitRegionGenerator()
    {
        DungeonBuilder.DungeonFinishedBuilding += DungeonBuilder_DungeonFinishedBuilding;
    }

    private void DungeonBuilder_DungeonFinishedBuilding()
    {
        GenerateProperties();
        Debug.Log("Done generating properties for: " + propertyKey);
    }

    protected virtual void GenerateProperties()
    {
        DungeonPiece seedPiece = PickSeedPiece();
        string initialProperty = (seedProperty == "") ? this.propertyPool.GetRandomObjectFromPool() : seedProperty;

        ApplyPropertyToPiece(initialProperty, seedPiece);
        ApplyPropertyToConnectingPieces(initialProperty, seedPiece);
    }

    List<DungeonPiece> uncheckedDungeonPieces;
    List<DungeonPiece> checkedDungeonPieces;
    protected virtual void ApplyPropertyToConnectingPieces(string initialProperty, DungeonPiece seedPiece)
    {
        string pieceProperty = initialProperty;
        uncheckedDungeonPieces = new List<DungeonPiece>();
        checkedDungeonPieces = new List<DungeonPiece>();

        CheckConnectedPieces(seedPiece);

        while(uncheckedDungeonPieces.Count > 0)
        {
            DungeonPiece pieceToCheck = uncheckedDungeonPieces[0];
            pieceProperty = GetNextProperty(pieceProperty);

            ApplyPropertyToPiece(pieceProperty, pieceToCheck);

            CheckConnectedPieces(pieceToCheck);

            uncheckedDungeonPieces.RemoveAt(0);
        }
    }

    protected virtual void CheckConnectedPieces(DungeonPiece centralPiece)
    {
        if (!checkedDungeonPieces.Contains(centralPiece))
        {
            checkedDungeonPieces.Add(centralPiece);
        }
        
        foreach (DungeonPiece connectedPiece in GetConnectedPieces(centralPiece))
        {
            if (!checkedDungeonPieces.Contains(connectedPiece))
            {
                uncheckedDungeonPieces.Add(connectedPiece);
            }
        }
    }

    protected List<DungeonPiece> GetConnectedPieces(DungeonPiece centralPiece)
    {
        List<DungeonPiece> connectedPieces = new List<DungeonPiece>();

        foreach(PieceConnection connection in centralPiece.ExitPieceConnections)
        {
            connectedPieces.Add(connection.connectedPiece);
        }

        return connectedPieces;
    }

    protected virtual DungeonPiece PickSeedPiece()
    {
        int r = Random.Range(0, this.dungeonTracker.SpawnedDungeonPieces.Count);

        return this.dungeonTracker.SpawnedDungeonPieces[r];
    }

    protected virtual string GetNextProperty(string previousProperty)
    {
        //TODO, Make this real...
        return this.propertyPool.GetRandomObjectFromPool();
    }

    protected virtual void ApplyPropertyToPiece(string propertyValue, DungeonPiece piece)
    {
        if (!piece.PieceProperties.ContainsKey(propertyKey))
        {
            piece.AddProperty(propertyKey, propertyValue);
            //Debug.Log("I applied this property: " + property + " to this room: " + piece.gameObject.name, piece.gameObject);
        } else
        {
            //The piece already had that property, should we combine || override || ignore?
            Debug.LogWarning("The piece already had this property: " + propertyKey + ". I don't know what the collision function is");
        }
    }


}
