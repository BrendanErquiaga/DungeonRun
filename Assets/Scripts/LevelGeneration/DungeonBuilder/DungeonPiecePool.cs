using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiecePool : MonoBehaviour
{
    [SerializeField]
    private List<GameObjectWithInt> dungeonPiecePool;
    [SerializeField]
    private List<GameObjectWithInt> wallPiecePool;
    [HideInInspector]
    public bool poolIsPrepared = false;

    private List<GameObjectWithInt> connectorOnlyPool;
    private List<GameObjectWithInt> nonRoomPiecePool;

    private int dungeonPieceSumOfChances;
    private int connectorOnlySumOfChances;
    private int nonRoomSumOfChances;
    private int wallPieceSumOfChances;

    public List<GameObjectWithInt> DungeonPieces
    {
        get
        {
            return dungeonPiecePool;
        }

        set
        {
            dungeonPiecePool = value;
        }
    }

    private void Awake()
    {
        CreatePools();
        CalculateShuffleTables();
        poolIsPrepared = true;
    }

    private void CreatePools()
    {
        connectorOnlyPool = new List<GameObjectWithInt>();
        nonRoomPiecePool = new List<GameObjectWithInt>();
        foreach (GameObjectWithInt dungeonPiece in dungeonPiecePool)
        {
            DungeonPieceType pieceType = dungeonPiece.objectToUse.GetComponent<DungeonPiece>().PieceType;
            if (pieceType == DungeonPieceType.CONNECTOR)
            {
                connectorOnlyPool.Add(dungeonPiece);
                nonRoomPiecePool.Add(dungeonPiece);
            } else if(pieceType == DungeonPieceType.INTERSECTION)
            {
                nonRoomPiecePool.Add(dungeonPiece);
            }
        }
    }

    private void CalculateShuffleTables()
    {
        dungeonPieceSumOfChances = GetSumOfChances(dungeonPiecePool);
        connectorOnlySumOfChances = GetSumOfChances(connectorOnlyPool);
        nonRoomSumOfChances = GetSumOfChances(nonRoomPiecePool);
        wallPieceSumOfChances = GetSumOfChances(wallPiecePool);
    }

    private int GetSumOfChances(List<GameObjectWithInt> gowiList)
    {
        int tempSum = 0;

        foreach (GameObjectWithInt gowi in gowiList)
        {
            tempSum += gowi.value;
        }

        return tempSum;
    }

    public GameObject GetDungeonPiece()
    {
        return GetRandomPieceFromPool(dungeonPiecePool, dungeonPieceSumOfChances);
    }

    public GameObject GetWallPiece()
    {
        return GetRandomPieceFromPool(wallPiecePool, wallPieceSumOfChances);
    }

    private GameObject GetRandomPieceFromPool(List<GameObjectWithInt> pool, int sumOfChances)
    {
        if (pool.Count == 1)
        {
            return pool[0].objectToUse;
        }

        int r = Random.Range(1, sumOfChances + 1);
        int previousFloor = 0;
        GameObject pieceToReturn = null;

        foreach (GameObjectWithInt gowi in pool)
        {
            if (r <= gowi.value + previousFloor)
            {
                pieceToReturn = gowi.objectToUse;
                break;
            }
            else
            {
                previousFloor += gowi.value;
            }
        }

        return pieceToReturn;
    }

    public GameObject GetDungeonPiece(DungeonPiece connectedDungeonPiece)
    {
        if (dungeonPiecePool.Count == 1)
        {
            return dungeonPiecePool[0].objectToUse;
        }

        GameObject pieceToReturn = null;

        switch (connectedDungeonPiece.PieceType)
        {
            case DungeonPieceType.INTERSECTION:
                pieceToReturn = GetRandomPieceFromPool(connectorOnlyPool, connectorOnlySumOfChances);
                break;
            default:
                pieceToReturn = GetRandomPieceFromPool(dungeonPiecePool, dungeonPieceSumOfChances);
                break;
        }

        return pieceToReturn;
    }
}
