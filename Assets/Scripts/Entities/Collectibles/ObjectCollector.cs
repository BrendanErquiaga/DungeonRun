using UnityEngine;
using System.Collections;
using System;

public class ObjectCollector : MonoBehaviour 
{
    public Collider objectCollectionCollider;
    protected bool canCollect = true;    
	
	public bool CanCollect { get { return canCollect; } }

    public Transform CollectionTarget
    {
        get
        {
            if (objectCollectionCollider != null)
                return objectCollectionCollider.transform;
            else
                return this.transform;
        }
    }

    public delegate void CollectorEvent(CollectibleObject collectibleObject);
    public static event CollectorEvent ObjectCollected;

    private void OnObjectCollected(CollectibleObject objectToCollect)
    {
        CollectorEvent handler = ObjectCollected;
        if (handler != null)
            handler(objectToCollect);
    }

    private void Awake()
    {
        if(objectCollectionCollider == null)
        {
            objectCollectionCollider = this.gameObject.GetComponent<Collider>();

            if(objectCollectionCollider == null)
            {
                Debug.LogError("ObjectCollector has no collider present");
            }
        }
    }

    public void CollectObject(CollectibleObject objectToCollect)
    {
        OnObjectCollected(objectToCollect);
    }
}
