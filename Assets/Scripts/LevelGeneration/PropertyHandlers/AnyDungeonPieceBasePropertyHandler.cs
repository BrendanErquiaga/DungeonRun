using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyDungeonPieceBasePropertyHandler : MonoBehaviour
{
    public string propertyKeyToMatch;

    private void OnEnable()
    {
        InitPropertyHandler();
    }

    protected void InitPropertyHandler()
    {
        DungeonPiece.AnyPieceDungeonPropertyAdded += HandlePropertyAdded;
    }

    protected virtual void HandlePropertyAdded(DungeonPiece dungeonPiece, string propertyKey,string property)
    {
        
    }

    protected virtual bool PropertyKeysMatch(string propertyKey)
    {
        return this.propertyKeyToMatch == propertyKey;
    }
}
