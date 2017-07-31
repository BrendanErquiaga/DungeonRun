using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizedPoolPropertyApplicator : DungeonPiecePropertyApplicator
{
    [SerializeField]
    [Range(0,1000)]
    protected int minPoolSize = 1;
    [SerializeField]
    [Range(1, 1000)]
    protected int maxPoolSize = 10;

    private int nextPoolSize;
    private int currentPoolCount = 0;

    protected override void InitDungeonPiecePropertyApplicator()
    {
        base.InitDungeonPiecePropertyApplicator();
        GenerateNextPoolSize();
    }

    protected virtual void GenerateNextPoolSize()
    {
        nextPoolSize = Random.Range(minPoolSize, maxPoolSize);
    }

    protected override string GetNextProperty(string previousProperty)
    {
        if(currentPoolCount < nextPoolSize)
        {
            currentPoolCount++;
            return previousProperty;
        }
        else
        {
            GenerateNextPoolSize();
            currentPoolCount = 0;
            this.dungeonFetcher.ReorderListWithNewSeed();
            return base.GetNextProperty(previousProperty);
        }      
    }
}
