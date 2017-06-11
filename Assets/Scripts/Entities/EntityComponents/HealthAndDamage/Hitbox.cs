using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private bool singleHitHitbox = true;

    [SerializeField]
    private bool disabledCollisionOnHit = false;

    [SerializeField]
    private bool enableVisualsWithCollision = false;    

    [SerializeField]
    private float timeDelayBetweenSameTargetHit = 1;

    private List<ColliderWithTimestamp> collidersHitThisAttack = new List<ColliderWithTimestamp>();

    private bool destroyedMessageSent = false;

    public bool SingleHitHitbox
    {
        get
        {
            return singleHitHitbox;
        }

        set
        {
            singleHitHitbox = value;
        }
    }

    public float TimeDelayBetweenSameTargetHit
    {
        get
        {
            return timeDelayBetweenSameTargetHit;
        }

        set
        {
            timeDelayBetweenSameTargetHit = value;
        }
    }

    public bool EnableVisualsWithCollision
    {
        get
        {
            return enableVisualsWithCollision;
        }

        set
        {
            enableVisualsWithCollision = value;
        }
    }

    public List<ColliderWithTimestamp> CollidersHitThisAttack
    {
        get
        {
            return collidersHitThisAttack;
        }

        protected set
        {
            collidersHitThisAttack = value;
        }
    }

    public bool DisabledCollisionOnHit
    {
        get
        {
            return disabledCollisionOnHit;
        }

        set
        {
            disabledCollisionOnHit = value;
        }
    }

    public delegate void HitboxCollidedEvent(Collider colliderHit);
    public event HitboxCollidedEvent AttackHit;
    public delegate void HitboxEvent();
    public event HitboxEvent AttackDestroyed;

    protected virtual void OnAttackHit(Collider colliderHit)
    {
        HitboxCollidedEvent handler = AttackHit;
        if (handler != null)
            handler(colliderHit);
    }

    protected virtual void OnAttackDestroyed()
    {
        if (destroyedMessageSent)
            return;

        HitboxEvent handler = AttackDestroyed;
        if (handler != null)
            handler();

        destroyedMessageSent = true;
    }

    protected virtual void OnTriggerEnter(Collider colliderHit)
    {
        if (CollidersCanHit(colliderHit))
            CollidersHit(colliderHit);
    }

    protected virtual bool CollidersCanHit(Collider colliderHit)
    {
        foreach (ColliderWithTimestamp colliderWithValue in collidersHitThisAttack)
            if (colliderHit == colliderWithValue.collider)
                return Time.timeSinceLevelLoad - colliderWithValue.timeStamp > timeDelayBetweenSameTargetHit;

        return true;
    }

    protected virtual void CollidersHit(Collider colliderHit)
    {
        OnAttackHit(colliderHit);

        collidersHitThisAttack.Add(new ColliderWithTimestamp(colliderHit, Time.timeSinceLevelLoad));

        if (SingleHitHitbox && DisabledCollisionOnHit)
            DisableCollision();
    }

    void OnDestroy()
    {
        OnAttackDestroyed();
    }

    public void EnableCollision()
    {
        GetComponent<Collider>().enabled = true;

        if (EnableVisualsWithCollision && GetComponent<Renderer>())
            GetComponent<Renderer>().enabled = true;
    }

    public virtual void DisableCollision()
    {
        GetComponent<Collider>().enabled = false;

        if (EnableVisualsWithCollision && GetComponent<Renderer>())
            GetComponent<Renderer>().enabled = false;

        collidersHitThisAttack.Clear();
    }

    public class ColliderWithTimestamp
    {
        public Collider collider;
        public float timeStamp;

        public ColliderWithTimestamp(Collider collider, float timeStamp)
        {
            this.collider = collider;
            this.timeStamp = timeStamp;
        }
    }
}
