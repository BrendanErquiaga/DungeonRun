using UnityEngine;
using System.Collections;

public class DestroyOnDelay : MonoBehaviour 
{
	[SerializeField] float delayTime = 2f;

	void Start()
	{
		Destroy(gameObject, delayTime);
	}
}
