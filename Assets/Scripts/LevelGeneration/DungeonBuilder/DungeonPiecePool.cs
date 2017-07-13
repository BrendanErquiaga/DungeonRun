using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiecePool : MonoBehaviour
{
    [SerializeField]
    private List<GameObjectWithInt> dungeonPieces;
    [HideInInspector]
    public bool poolIsPrepared = false;

    private int sumOfChances;

    public List<GameObjectWithInt> DungeonPieces
    {
        get
        {
            return dungeonPieces;
        }

        set
        {
            dungeonPieces = value;
        }
    }

    private void Awake()
    {
        CalculateShuffleTable();
        poolIsPrepared = true;
    }

    private void CalculateShuffleTable()
    {
        int tempSum = 0;

        foreach (GameObjectWithInt dungeonPiece in dungeonPieces)
        {
            tempSum += dungeonPiece.value;
        }

        sumOfChances = tempSum;
    }

    public GameObject GetDungeonPiece()
    {
        if (dungeonPieces.Count == 1)
        {
            return dungeonPieces[0].objectToUse;
        }

        int r = Random.Range(1, sumOfChances + 1);
        int previousFloor = 0;
        GameObject pieceToReturn = null;        

        foreach (GameObjectWithInt dungeonPiece in dungeonPieces)
        {
            if (r <= dungeonPiece.value + previousFloor)
            {
                pieceToReturn = dungeonPiece.objectToUse;
                break;
            }
            else
            {
                previousFloor += dungeonPiece.value;
            }
        }

        return pieceToReturn;
    }

    public GameObject GetDungeonPiece(DungeonPiece lastDungeonPiece)
    {
        if (dungeonPieces.Count == 1)
        {
            return dungeonPieces[0].objectToUse;
        }

        int checks = 0;
        GameObject pieceToReturn = null;
        DungeonPieceType typeOfReturnPiece;

        do
        {
            pieceToReturn = GetDungeonPiece();
            typeOfReturnPiece = pieceToReturn.GetComponent<DungeonPiece>().PieceType;
            checks++;
        } while (!CanConnectPieces(lastDungeonPiece, typeOfReturnPiece) && checks < 100);

        if(checks >= 100)
        {
            Debug.Log("This thing tried really hard to find the correct piece but couldn't =/.");
        }

        return pieceToReturn;
    }

    public bool CanConnectPieces(DungeonPiece pieceToConnectTo, DungeonPieceType typeOfConnectorPiece)
    {
        bool canConnectPieces = false;

        switch (pieceToConnectTo.PieceType)
        {
            case DungeonPieceType.INTERSECTION:
                if(typeOfConnectorPiece == DungeonPieceType.CONNECTOR)
                {
                    canConnectPieces = true;
                }
                break;
            case DungeonPieceType.ROOM:
                if (typeOfConnectorPiece == DungeonPieceType.CONNECTOR || typeOfConnectorPiece == DungeonPieceType.INTERSECTION)
                {
                    canConnectPieces = true;
                }
                break;
            default:
                canConnectPieces = true;
                break;
        }

        return canConnectPieces;
    }
}
