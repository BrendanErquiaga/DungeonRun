using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayingPropertyApplicator : DungeonPiecePropertyApplicator
{
    [SerializeField]
    protected float propertyRepeatChance = 1;
    [SerializeField]
    protected float repeatChanceDecayRate = 1;

    [SerializeField]
    protected bool reorderOnPropertyChange = true;

    protected float currentRepeatChance;

    protected override void InitDungeonPiecePropertyApplicator()
    {
        base.InitDungeonPiecePropertyApplicator();
        currentRepeatChance = propertyRepeatChance;
    }

    protected override string GetNextProperty(string previousProperty)
    {
        if (RepeatProperty())
        {
            currentRepeatChance *= repeatChanceDecayRate;
            return previousProperty;
        } else
        {
            if (reorderOnPropertyChange)
            {
                this.dungeonFetcher.ReorderListWithNewSeed();
            }
            currentRepeatChance = propertyRepeatChance;
            return this.propertyPool.GetDifferentRandomObjectFromPool(previousProperty);
        }
    }

    protected bool RepeatProperty()
    {
        float r = Random.Range(0, 100);

        return (r <= currentRepeatChance * 100);
    }
}
