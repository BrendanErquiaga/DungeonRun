using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiecePropertyApplicator : MonoBehaviour
{
    [SerializeField]
    protected DungeonPieceTracker dungeonTracker;
    [SerializeField]
    protected PropertyPool propertyPool;
    [SerializeField]
    protected string propertyKey;
    [SerializeField]
    protected PropertyCollisionBehavior propertyCollisionBehavior;

    protected List<DungeonPiece> uncheckedDungeonPieces;
    protected List<DungeonPiece> checkedDungeonPieces;

    private void Start()
    {
        InitDungeonPiecePropertyApplicator();
    }

    protected virtual void InitDungeonPiecePropertyApplicator()
    {

    }

    public virtual void ApplyPropertiesToAllDungeonPieces()
    {
        DungeonPiece seedPiece = PickSeedPiece();
        string initialProperty = this.GetNextProperty();

        ApplyPropertyToPiece(initialProperty, seedPiece);
        ApplyPropertyToConnectingPieces(initialProperty, seedPiece);
    }

    
    protected virtual void ApplyPropertyToConnectingPieces(string initialProperty, DungeonPiece seedPiece)
    {
        string pieceProperty = initialProperty;
        this.uncheckedDungeonPieces = new List<DungeonPiece>();
        this.checkedDungeonPieces = new List<DungeonPiece>();

        CheckConnectedPieces(seedPiece);

        while(this.uncheckedDungeonPieces.Count > 0)
        {
            DungeonPiece pieceToCheck = this.uncheckedDungeonPieces[0];
            pieceProperty = GetNextProperty(pieceProperty);

            ApplyPropertyToPiece(pieceProperty, pieceToCheck);

            CheckConnectedPieces(pieceToCheck);

            this.uncheckedDungeonPieces.RemoveAt(0);
        }
    }

    protected virtual void CheckConnectedPieces(DungeonPiece centralPiece)
    {
        if (!this.checkedDungeonPieces.Contains(centralPiece))
        {
            this.checkedDungeonPieces.Add(centralPiece);
        }
        
        foreach (DungeonPiece connectedPiece in GetConnectedPieces(centralPiece))
        {
            if (!this.checkedDungeonPieces.Contains(connectedPiece))
            {
                this.uncheckedDungeonPieces.Add(connectedPiece);
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

    protected virtual string GetNextProperty()
    {
        return this.propertyPool.GetRandomObjectFromPool();
    }

    protected virtual string GetNextProperty(string previousProperty)
    {
        return this.propertyPool.GetRandomObjectFromPool();
    }

    protected virtual void ApplyPropertyToPiece(string propertyValue, DungeonPiece piece)
    {
        if (!piece.PieceProperties.ContainsKey(propertyKey))
        {
            piece.AddProperty(propertyKey, propertyValue);
        } else
        {
            HandlePropertyCollision(propertyValue, piece);
        }
    }

    protected virtual void HandlePropertyCollision(string propertyValue, DungeonPiece piece)
    {
        switch (propertyCollisionBehavior)
        {
            case PropertyCollisionBehavior.COMBINE:
                string combinedProperty = piece.PieceProperties[propertyKey] + " " + propertyValue;
                piece.AddProperty(propertyKey, combinedProperty);
                break;
            case PropertyCollisionBehavior.OVERRIDE:
                piece.AddProperty(propertyKey, propertyValue);
                break;
            case PropertyCollisionBehavior.SKIP:
                //DO NOTHING
                break;
            default:
                Debug.LogWarning("Unhandled Behavior found");
                break;
        }
    }
}

public enum PropertyCollisionBehavior
{
    SKIP,
    COMBINE,
    OVERRIDE    
}
