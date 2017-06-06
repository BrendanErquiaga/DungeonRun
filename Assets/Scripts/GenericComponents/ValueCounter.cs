using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounter : MonoBehaviour {

    [SerializeField]
    private float startingValue = 0;

    [SerializeField]
    private bool roundToInt = false;

    private float currentValue;
    private bool hasBeenInitialized = false;

    public float StartingValue
    {
        get
        {
            return startingValue;
        }

        protected set
        {
            startingValue = value;
        }
    }

    public virtual float CurrentValue
    {
        get
        {
            if (!hasBeenInitialized)
            {
                HandleInitialization();
            }

            return (RoundToInt) ? Mathf.Round(currentValue) : currentValue;
        }

        protected set
        {
            if (!hasBeenInitialized)
            {
                HandleInitialization();
            }

            if (currentValue != value)
            {
                currentValue = value;
                OnValueChanged();
            }

            currentValue = value;
        }
    }

    public bool RoundToInt
    {
        get
        {
            return roundToInt;
        }

        set
        {
            roundToInt = value;
        }
    }

    public delegate void ValueCounterEvent();
    public event ValueCounterEvent ValueChanged;

    protected virtual void OnValueChanged()
    {
        ValueCounterEvent handler = ValueChanged;

        if (handler != null)
        {
            handler();
        }            
    }

    protected virtual void HandleInitialization()
    {
        if (!hasBeenInitialized)
        {
            hasBeenInitialized = true;
            CurrentValue = StartingValue;
        }
    }

    /// <summary>
    /// Sets the current value to a new number
    /// </summary>
    /// <param name="newValue">The new number</param>
    public void SetValue(float newValue)
    {
        CurrentValue = newValue;
    }

    /// <summary>
    /// Adds a number to the current value
    /// </summary>
    /// <param name="numberToAdd">The number to add</param>
    public virtual void AddToValue(float numberToAdd)
    {
        CurrentValue += numberToAdd;
    }

    /// <summary>
    /// Subtracts a number from the current value
    /// </summary>
    /// <param name="numberToSubtract">The number to subtract</param>
    public virtual void SubtractFromValue(float numberToSubtract)
    {    
        CurrentValue -= numberToSubtract;
    }
}
