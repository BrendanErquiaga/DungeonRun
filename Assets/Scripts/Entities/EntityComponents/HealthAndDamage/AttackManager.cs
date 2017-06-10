using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private string description = "Default Attack";

    [SerializeField]
    private float attackDamage;

    public float AttackDamage
    {
        get
        {
            return attackDamage;
        }

        set
        {
            attackDamage = value;
        }
    }

    protected string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public virtual void HitDefenseManager(DefenseManager defenseManager)
    {
        defenseManager.TakeDamage(AttackDamage);
    }
}
