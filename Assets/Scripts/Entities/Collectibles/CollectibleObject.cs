using UnityEngine;
using System.Collections.Generic;

public class CollectibleObject : MonoBehaviour
{
	#region Members
	[SerializeField] AwarenessZoneListener awarenessZoneListener;
	[SerializeField] float magnetStrength = 0.5f;
	[SerializeField] float magnetDistance = 3f;
	[SerializeField] float collectDistance = 0.5f;
	[SerializeField] float collectDelay = 0.1f;
	[SerializeField] float velocityDampening = 0.5f;
	[SerializeField] float onStartIgnoreCollectionTime = 0.5f;
    [SerializeField] ForceMode forceMode = ForceMode.VelocityChange;

    private float currentDistanceToTarget;
    private float collectorIgnoreListClearRate = 5f;//How frequently we clear the ignored object collectors list	
	private Transform magnetTarget;
	private ObjectCollector targetObjectCollector;
	private bool hasBeenCollected = false;
	private bool collectionEnabled = true;
	private List<ObjectCollector> collectorsToIgnore = new List<ObjectCollector>();
	#endregion

	#region Properties
	public bool CollectionEnabled 
	{
		get 
		{
			return collectionEnabled;
		}
		set 
		{
			collectionEnabled = value;
		}
	}
	#endregion

	#region Events
	public delegate void CollectibleObjectEvent(CollectibleObject collectibleObject);
	public static event CollectibleObjectEvent ObjectCollected;
	public event CollectibleObjectEvent TargetAcquired;
	public event CollectibleObjectEvent TargetRemoved;

	protected virtual void OnObjectCollected (CollectibleObject collectibleObject)
	{
		hasBeenCollected = true;

		CollectibleObjectEvent handler = ObjectCollected;
		if (handler != null)
			handler (collectibleObject);
	}

	protected virtual void OnTargetAcquired (CollectibleObject collectibleObject)
	{
		CollectibleObjectEvent handler = TargetAcquired;
		if (handler != null)
			handler (collectibleObject);
	}

	protected virtual void OnTargetRemoved (CollectibleObject collectibleObject)
	{
		CollectibleObjectEvent handler = TargetRemoved;
		if (handler != null)
			handler (collectibleObject);
	}
	#endregion

	private void OnEnable()
	{
		InitializeEventListeners();
		InvokeRepeating("ClearIgnoreList",collectorIgnoreListClearRate,collectorIgnoreListClearRate);
	}

	private void OnDisable()
	{
		DisableEventListeners();
		CancelInvoke("ClearIgnoreList");
	}

	protected virtual void InitializeEventListeners ()
	{
		awarenessZoneListener.EnteredAwarenessZone += HandleEnteredAwarenessZone;
		awarenessZoneListener.StayedInAwarenessZone += HandleEnteredAwarenessZone;
		awarenessZoneListener.ExitedAwarenessZone += HandleExitedAwarenessZone;
	}

	protected virtual void DisableEventListeners ()
	{
		awarenessZoneListener.EnteredAwarenessZone -= HandleEnteredAwarenessZone;
		awarenessZoneListener.StayedInAwarenessZone -= HandleEnteredAwarenessZone;
		awarenessZoneListener.ExitedAwarenessZone -= HandleExitedAwarenessZone;
	}

	public void ClearIgnoreList()
	{
		collectorsToIgnore.Clear();
	}

	private void Awake()
	{
		CollectionEnabled = false;		
		Invoke("EnableCollection", onStartIgnoreCollectionTime);
	}

	private void EnableCollection()
	{
		CollectionEnabled = true;
	}

	protected virtual void Update()
	{
		if(hasBeenCollected || !CollectionEnabled)
			return;

		if(magnetTarget != null && !collectorsToIgnore.Contains(targetObjectCollector))
			TrackToTarget();
	}

	protected virtual void TrackToTarget()
	{
        currentDistanceToTarget = Vector3.Distance(transform.position, magnetTarget.position);

        if (currentDistanceToTarget <= magnetDistance)
		{
            if (GetComponent<Rigidbody>().velocity == Vector3.zero)
			{
				OnTargetAcquired(this);

				if(magnetTarget == null)
					return;
			}

			if(currentDistanceToTarget <= collectDistance)
				CollectObject(targetObjectCollector);
			else
			{
				Vector3 moveDirection = magnetTarget.position - transform.position;                

                GetComponent<Rigidbody>().velocity *= velocityDampening;
				GetComponent<Rigidbody>().AddForce(moveDirection.normalized * CalculateMagnetStrength() * Time.deltaTime, this.forceMode);
			}
		}
	}

    protected virtual float CalculateMagnetStrength()
    {
        float newMagnetSpeed = (magnetDistance / currentDistanceToTarget) * magnetStrength;
        newMagnetSpeed = Mathf.Clamp(newMagnetSpeed, this.magnetStrength / 10, this.magnetStrength * 10);
        //Debug.Log("MagSpeed: " + newMagnetSpeed);
        return newMagnetSpeed;
    }

	protected virtual void CollectObject(ObjectCollector objectCollector)
	{
        Physics.IgnoreCollision(GetComponent<Collider>(), objectCollector.objectCollectionCollider);
        OnObjectCollected(this);
        objectCollector.CollectObject(this);
		ClearMagnetTarget();

		Invoke("DestroyObject", collectDelay);
	}
	
	protected virtual void DestroyObject()
	{
		Destroy(gameObject);
	}

	void HandleEnteredAwarenessZone (AwarenessZone awarenessZone)
	{
		if(UseNewAwarenessZone(awarenessZone))
		{
            ObjectCollector objectCollector = Utilities.GetObjectOfType<ObjectCollector>(awarenessZone.gameObject);
			
			if(objectCollector != null && objectCollector.CanCollect 
			   && targetObjectCollector != objectCollector
               && !collectorsToIgnore.Contains(objectCollector))
			{
				magnetTarget = objectCollector.CollectionTarget;
				targetObjectCollector = objectCollector;
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), objectCollector.objectCollectionCollider, true);

                OnTargetAcquired(this);
			}
		}
	}

	private bool UseNewAwarenessZone(AwarenessZone awarenessZone)
	{
		if(magnetTarget == null)
			return true;

		float distanceToOldTarget = Vector3.Distance(transform.position, magnetTarget.position);
		float distanceToNewTarget = Vector3.Distance(transform.position, awarenessZone.transform.position);

		if(distanceToNewTarget < distanceToOldTarget)
			return true;
		else
			return false;
	}
	

	void HandleExitedAwarenessZone (AwarenessZone awarenessZone)
	{
		if(magnetTarget != null && magnetTarget == awarenessZone)
		{
			magnetTarget = null;
			targetObjectCollector = null;
		}
	}

	public virtual void ClearMagnetTarget()
	{
		if(magnetTarget != null)
		{
            Physics.IgnoreCollision(GetComponent<Collider>(), targetObjectCollector.objectCollectionCollider, false);
            magnetTarget = null;
			OnTargetRemoved(this);
			targetObjectCollector = null;
		}
	}

	public virtual void IgnoreObjectCollector(ObjectCollector collectorToIgnore)
	{
		ClearMagnetTarget();
		if(!collectorsToIgnore.Contains(collectorToIgnore))
		{
			collectorsToIgnore.Add(collectorToIgnore);
		}
	}
}
