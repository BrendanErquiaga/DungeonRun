using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManagerHitbox : Hitbox {

    [SerializeField]
    private AttackManager attackManager;

    public AttackManager AttackManager
    {
        get
        {
            return attackManager;
        }

        set
        {
            attackManager = value;
        }
    }

    protected override bool CollidersCanHit(Collider colliderHit)
    {
        return (GetDefenseManager(colliderHit.gameObject) != null) ? base.CollidersCanHit(colliderHit) : false;
    }

    protected override void CollidersHit(Collider colliderHit)
    {
        base.CollidersHit(colliderHit);
        DefenseManager defenseManagerHit = GetDefenseManager(colliderHit.gameObject);

        if(defenseManagerHit != null)
            AttackManager.HitDefenseManager(defenseManagerHit);
    }

    protected virtual DefenseManager GetDefenseManager(GameObject objectHit)
    {
        DefenseManager defenseManager = objectHit.GetComponent<DefenseManager>();

        if (defenseManager == null)
        {
            DefenseManagerHurtbox defenseManagerHurtbox = objectHit.GetComponent<DefenseManagerHurtbox>();

            if (defenseManagerHurtbox != null)
                defenseManager = defenseManagerHurtbox.DefenseManager;
        }

        return defenseManager;
    }
}
