using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPieceBasePropertyHandler : MonoBehaviour
{
    public string propertyKeyToMatch;
    protected DungeonPiece parentDungeonPiece;

    private void Awake()
    {
        InitPropertyHandler();
    }

    protected void InitPropertyHandler()
    {
        if(parentDungeonPiece != null)
        {
            parentDungeonPiece.DungeonPropertyAdded += HandlePropertyAdded;
        } else
        {
            Debug.LogWarning("No parent dungeon piece on this Dungeon Piece property handler");
        }
    }

    private void HandlePropertyAdded(DungeonPiece dungeonPiece, string propertyKey, string property)
    {
        
    }

    protected virtual bool PropertyKeysMatch(string propertyKey)
    {
        return this.propertyKeyToMatch == propertyKey;
    }
}
