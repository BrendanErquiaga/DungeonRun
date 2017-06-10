using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    protected List<Hitbox> hitboxesHit = new List<Hitbox>();

    public delegate void HurtboxEvent();
    public event HurtboxEvent HurtboxHit;

    public delegate void HurtboxCollidedEvent(Hitbox hitbox);
    public event HurtboxCollidedEvent HurtboxCollided;

    protected virtual void OnHurtboxHit()
    {
        HurtboxEvent handler = HurtboxHit;

        if (handler != null)
            handler();
    }

    protected virtual void OnHurtboxCollided(Hitbox hitbox)
    {
        HurtboxCollidedEvent handler = HurtboxCollided;

        if (handler != null)
            handler(hitbox);
    }

    protected virtual void OnTriggerEnter(Collider colliderHit)
    {
        OnHurtboxHit();
        Hitbox hitbox = colliderHit.GetComponent<Hitbox>();
        if (hitbox != null)
        {
            hitboxesHit.Add(hitbox);
            OnHurtboxCollided(hitbox);
        }
    }
}
