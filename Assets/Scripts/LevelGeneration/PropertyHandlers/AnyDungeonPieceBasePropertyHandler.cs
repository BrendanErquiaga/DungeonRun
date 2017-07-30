using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyDungeonPieceBasePropertyHandler : MonoBehaviour
{
    private void Awake()
    {
        InitPropertyHandler();
    }

    protected void InitPropertyHandler()
    {
        DungeonPiece.AnyPieceDungeonPropertyAdded += HandlePropertyAdded;
    }

    protected virtual void HandlePropertyAdded(DungeonPiece dungeonPiece, string property)
    {
        
    }
}
