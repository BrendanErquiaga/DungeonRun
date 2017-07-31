using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiecePropertyApplicator : MonoBehaviour
{
    [SerializeField]
    protected DungeonPieceFetcher dungeonFetcher;
    [SerializeField]
    protected PropertyPool propertyPool;
    [SerializeField]
    protected string propertyKey;
    [SerializeField]
    protected PropertyCollisionBehavior propertyCollisionBehavior;

    private void Start()
    {
        InitDungeonPiecePropertyApplicator();
    }

    protected virtual void InitDungeonPiecePropertyApplicator()
    {

    }

    public virtual void ApplyPropertiesToAllDungeonPieces()
    {
        string nextProperty = this.GetNextProperty();
        while (this.dungeonFetcher.CanFetch)
        {
            nextProperty = GetNextProperty(nextProperty);
            DungeonPiece nextPiece = this.dungeonFetcher.FetchNextObject();
            ApplyPropertyToPiece(nextProperty, nextPiece);
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
        return this.dungeonFetcher.FetchNextObject();
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
