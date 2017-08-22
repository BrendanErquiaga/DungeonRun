using UnityEngine;
using System.Collections;

public class AwarenessZoneListener : MonoBehaviour 
{
	public delegate void AwarenessZoneEvent(AwarenessZone awarenessZone);
	public event AwarenessZoneEvent EnteredAwarenessZone;
	public event AwarenessZoneEvent StayedInAwarenessZone;
	public event AwarenessZoneEvent ExitedAwarenessZone;

	public void OnEnteredAwarenessZone (AwarenessZone awarenessZone)
	{
		AwarenessZoneEvent handler = EnteredAwarenessZone;
		if (handler != null)
			handler (awarenessZone);
	}

	public void OnStayedInAwarenessZone (AwarenessZone awarenessZone)
	{
		AwarenessZoneEvent handler = StayedInAwarenessZone;
		if (handler != null)
			handler (awarenessZone);
	}

	public void OnExitedAwarenessZone (AwarenessZone awarenessZone)
	{
		AwarenessZoneEvent handler = ExitedAwarenessZone;
		if (handler != null)
			handler (awarenessZone);
	}
}
