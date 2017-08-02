using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTopDownCamera : MonoBehaviour
{
    public Transform targetLookAt;
    public float distance = 7f;
    public float distanceMoveRate = 2f;
    public float xSmooth = 0.05f;
    public float ySmooth = 0.1f;
    public float offsetZ = 5;
    public bool lookAtTarget = false;
    [HideInInspector]
    public Camera cameraToUse;
   
    protected float startDistance = 0f;
    protected float desiredDistance = 0f;
    protected float velX = 0f;
    protected float velY = 0f;
    protected float velZ = 0f;
    protected float velDistance = 0f;
    protected Vector3 position = Vector3.zero;
    protected Vector3 desiredPosition = Vector3.zero;
    protected float distanceSmoothCurrent = 0f;

    public void Start()
    {
        BasicCamerInit();
    }

    protected virtual void BasicCamerInit()
    {
        this.cameraToUse = this.gameObject.GetComponent<Camera>();
        this.startDistance = this.distance;
        this.desiredDistance = this.distance;
    }

    public virtual void LateUpdate()
    {
        if (targetLookAt == null)
            return;

        CalculateDesiredPosition();

        UpdatePosition();
    }

    protected virtual void CalculateDesiredPosition()
    {
        this.desiredPosition = new Vector3(this.targetLookAt.position.x, this.targetLookAt.position.y + this.distance, this.targetLookAt.position.z - this.offsetZ);
    }

    protected virtual void UpdatePosition()
    {
        var posX = Mathf.SmoothDamp(this.position.x, this.desiredPosition.x, ref this.velX, this.xSmooth);
        var posY = Mathf.SmoothDamp(this.position.y, this.desiredPosition.y, ref this.velY, this.ySmooth);
        var posZ = Mathf.SmoothDamp(this.position.z, this.desiredPosition.z, ref this.velZ, this.xSmooth);
        this.position = new Vector3(posX, posY, posZ);

        this.transform.position = this.position;
        if(this.lookAtTarget)
        {
            this.transform.LookAt(this.targetLookAt);
        }        
    }
}
