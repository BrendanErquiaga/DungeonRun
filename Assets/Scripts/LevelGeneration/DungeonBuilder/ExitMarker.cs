using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMarker : MonoBehaviour
{
    private DungeonPiece parentDungeonPiece;

    public DungeonPiece ParentDungeonPiece
    {
        get
        {
            return parentDungeonPiece;
        }

        set
        {
            parentDungeonPiece = value;
        }
    }
}
