using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseManager : ValueCounterChanger
{
    public delegate void DamageTakenEvent(float damageTaken);
    public event DamageTakenEvent DamageTaken;

    protected virtual void OnDamageTaken(float damage)
    {
        DamageTakenEvent handler = DamageTaken;
        if (handler != null)
            handler(damage);
    }

    public virtual void TakeDamage(float damageToTake)
    {
        float calculatedDamage = CalculateDamageTaken(damageToTake);

        if (calculatedDamage > 0)
        {
            base.SubtractFromValue(calculatedDamage);

            OnDamageTaken(calculatedDamage);
        }
    }

    public virtual float CalculateDamageTaken(float baseDamage)
    {
        float damageTaken = baseDamage;

        return damageTaken;
    }
}
