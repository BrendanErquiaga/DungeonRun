using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadAndDecayPropertyApplicator : DungeonPiecePropertyApplicator
{
    [SerializeField]
    protected float propertyRepeatChance = 1;
    [SerializeField]
    protected float repeatChanceDecayRate = 1;

    protected float currentRepeatChance;

    protected override void InitDungeonPiecePropertyApplicator()
    {
        base.InitDungeonPiecePropertyApplicator();
        currentRepeatChance = propertyRepeatChance;
        Debug.Log("meep");
    }

    protected override string GetNextProperty(string previousProperty)
    {
        if (RepeatProperty())
        {
            currentRepeatChance *= repeatChanceDecayRate;
            return previousProperty;
        } else
        {
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
