using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPool : WeightedObjectPool<ObjectWithInt<string>, string>
{
    public List<StringWithInt> properties;

    string lastGeneratedProperty = "";

    private void Awake()
    {
        InitPropertyPool();
    }

    protected virtual void InitPropertyPool()
    {
        this.ObjectPool = new List<ObjectWithInt<string>>();
        foreach(StringWithInt property in properties)
        {
            this.ObjectPool.Add(new ObjectWithInt<string>(property.objectToUse, property.value));
        }

        base.CalculatePoolSize();
    }
}
