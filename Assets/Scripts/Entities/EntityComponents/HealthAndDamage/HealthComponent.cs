using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : ValueCounter
{
    [SerializeField]
    private float aliveThreshold = 1;//If below this value, no longer alive

    [SerializeField]
    private bool clampToZero = true;//Prevents the health value from going below zero

    private float maxHealth;
    private bool isAlive;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        protected set
        {
            isAlive = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return !IsAlive;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }

    public float CurrentHealthPercentage
    {
        get
        {
            return CurrentValue / MaxHealth;
        }
    }

    public override float CurrentValue
    {
        get
        {
            return base.CurrentValue;
        }

        protected set
        {
            base.CurrentValue = (ClampToZero) ? Mathf.Clamp(value, 0, MaxHealth) : Mathf.Min(value, MaxHealth);
            CheckForDeath();
        }
    }

    public float MinimumHealth
    {
        get
        {
            return aliveThreshold;
        }

        set
        {
            aliveThreshold = value;
        }
    }

    public bool ClampToZero
    {
        get
        {
            return clampToZero;
        }

        set
        {
            clampToZero = value;
        }
    }

    public delegate void HealthEvent();
    public event HealthEvent Healed;
    public event HealthEvent HealthValueChanged;
    public event HealthEvent Died;

    protected virtual void OnHealed()
    {
        HealthEvent handler = Healed;
        if (handler != null)
            handler();
    }

    protected virtual void OnHealthValueChanged()
    {
        HealthEvent handler = HealthValueChanged;
        if (handler != null)
            handler();
    }

    protected virtual void OnDied()
    {
        HealthEvent handler = Died;
        if (handler != null)
            handler();
    }

    protected override void OnValueChanged()
    {
        base.OnValueChanged();
        OnHealthValueChanged();
    }

    protected override void HandleInitialization()
    {
        MaxHealth = StartingValue;
        IsAlive = true;
        base.HandleInitialization();
    }

    public override void AddToValue(float numberToAdd)
    {
        base.AddToValue(numberToAdd);
        if (numberToAdd > 0)
        {
            OnHealed();
        }
    }

    public void ResetHealthValues()
    {
        SetHealthToMax();
    }

    public void SetHealthToMax()
    {
        if (CurrentValue != MaxHealth)
        {
            CurrentValue = MaxHealth;
        }
    }

    public void CheckForDeath()
    {
        if(IsAlive && CurrentValue < aliveThreshold)
        {
            IsAlive = false;
            OnDied();
        }
    }    
}
