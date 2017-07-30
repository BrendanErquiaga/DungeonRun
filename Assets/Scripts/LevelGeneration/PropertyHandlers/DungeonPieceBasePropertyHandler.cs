using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPieceBasePropertyHandler : MonoBehaviour
{

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

    private void HandlePropertyAdded(DungeonPiece dungeonPiece, string property)
    {
        
    }
}
