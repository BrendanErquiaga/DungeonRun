using UnityEngine;
using System.Collections.Generic;

public class AwarenessZone : MonoBehaviour 
{
	[SerializeField] protected LayerMask layersToAccept;
	protected List<GameObject> objectsInZone = new List<GameObject>();

	protected virtual void OnTriggerEnter(Collider colliderEntering)
	{
		if (layersToAccept.Contains(colliderEntering.gameObject.layer))
		{
			AddObject(colliderEntering.gameObject);
			AwarenessZoneListener awarenessZoneListener = colliderEntering.GetComponent<AwarenessZoneListener>();
			if (awarenessZoneListener != null)
				awarenessZoneListener.OnEnteredAwarenessZone(this);
		}
	}

	protected virtual void OnTriggerStay(Collider colliderStaying)
	{
		if (layersToAccept.Contains(colliderStaying.gameObject.layer))
		{
			AwarenessZoneListener awarenessZoneListener = colliderStaying.GetComponent<AwarenessZoneListener>();
			if (awarenessZoneListener != null)
				awarenessZoneListener.OnStayedInAwarenessZone(this);
		}
	}

	protected virtual void AddObject(GameObject gameObjectToAdd)
	{
		objectsInZone.Add(gameObjectToAdd);
	}

	protected virtual void RemoveObject(int index)
	{
		objectsInZone.RemoveAt(index);
	}

	public virtual void RemoveObject(GameObject gameObjectToRemove)
	{
		objectsInZone.Remove(gameObjectToRemove);
	}

	protected virtual void OnTriggerExit(Collider colliderExiting)
	{
		for (int index = objectsInZone.Count - 1; index >= 0; index--) 
		{		
			if (objectsInZone[index] == null || objectsInZone[index] == colliderExiting.gameObject)
			{
				if (objectsInZone[index] != null)
				{			
					AwarenessZoneListener awarenessZoneListener = colliderExiting.GetComponent<AwarenessZoneListener>();
					if (awarenessZoneListener != null)
						awarenessZoneListener.OnExitedAwarenessZone(this);
				}
				RemoveObject(index);
			}
		}
	}

	void OnDisable()
	{
		for (int index = objectsInZone.Count - 1; index >= 0; index--) 
		{
			RemoveObject(index);
		}
	}

	public List<T> GetObjectsOfType<T>(bool checkChildren = false) where T: Component
	{
		List<T> objectsToReturn = new List<T>();
		for (int index = 0; index < objectsInZone.Count; index++) 
		{
			if(objectsInZone[index] == null)
				continue;

			T potentialObject = objectsInZone[index].GetComponent<T>();
			if (potentialObject)
				objectsToReturn.Add(potentialObject);
			else if (checkChildren)
			{
				potentialObject = objectsInZone[index].GetComponentInChildren<T>();
				if (potentialObject)
					objectsToReturn.Add(potentialObject);
			}
		}

		return objectsToReturn;
	}
}
