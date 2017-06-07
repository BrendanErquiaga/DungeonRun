using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounterChanger : MonoBehaviour {

    [SerializeField]
    private ValueCounter valueCounter;

    public ValueCounter ValueCounter
    {
        get
        {
            return valueCounter;
        }

        set
        {
            valueCounter = value;
        }
    }

    public virtual void SetValue(float newValue)
    {
        ValueCounter.SetValue(newValue);
    }

    public virtual void AddToValue(float numberToAdd)
    {
        ValueCounter.AddToValue(numberToAdd);
    }

    public virtual void SubtractFromValue(float numberToSubtract)
    {
        ValueCounter.SubtractFromValue(numberToSubtract);
    }
}
