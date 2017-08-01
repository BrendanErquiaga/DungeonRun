using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTopDownCamera : MonoBehaviour
{
    public Transform targetLookAt;
    public float distance = 7f;
    public float distanceMoveRate = 2f;
    public float xSmooth = 0.05f;
    public float offsetZ = 5;

    private float startDistance = 0f;
    private float desiredDistance = 0f;
    private float velX = 0f;
    private float velY = 0f;
    private float velZ = 0f;
    private float velDistance = 0f;
    private Vector3 position = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;
    private float distanceSmoothCurrent = 0f;

    public void LateUpdate()
    {
        if (targetLookAt == null)
            return;

        CalculateDesiredPosition();

        UpdatePosition();
    }

    private void CalculateDesiredPosition()
    {
        this.desiredPosition = new Vector3(this.targetLookAt.position.x, this.targetLookAt.position.y + this.distance, this.targetLookAt.position.z - this.offsetZ);
    }

    private void UpdatePosition()
    {
        var posX = Mathf.SmoothDamp(this.position.x, this.desiredPosition.x, ref this.velX, this.xSmooth);
        var posY = Mathf.SmoothDamp(this.position.y, this.desiredPosition.y, ref this.velY, this.xSmooth);
        var posZ = Mathf.SmoothDamp(this.position.z, this.desiredPosition.z, ref this.velZ, this.xSmooth);
        this.position = new Vector3(posX, posY, posZ);

        this.transform.position = this.position;
    }
}
