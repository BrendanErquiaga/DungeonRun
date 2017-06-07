using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public Vector2 randomSpeed = new Vector2(-5,5);
	private float speed;
	public Vector3 spinAxis = new Vector3(0,0,1);
	private Vector3 spinAxisActual;

	private void Awake()
	{
		this.speed = Random.Range(this.randomSpeed.x, this.randomSpeed.y);

		int rotateClockWise = Random.Range(0,1);

		if(rotateClockWise == 0)
			this.spinAxisActual = this.spinAxis;
		else
			this.spinAxisActual = -this.spinAxis;
	}
	
	void FixedUpdate ()
	{
		this.transform.Rotate((this.spinAxisActual * this.speed) * Time.deltaTime);
	}
}
