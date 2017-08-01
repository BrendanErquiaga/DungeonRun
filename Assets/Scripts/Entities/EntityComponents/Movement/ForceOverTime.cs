using UnityEngine;
[System.Serializable]
public class ForceOverTime
{
    public Vector3 forceDirection;
    public float forceDistance;
    public float timeToApplyForceOver;
    public float timeElapsed = 0;

    public ForceOverTime(Vector3 forceDirection, float forceDistance, float timeToApplyForceOver)
    {
        this.forceDirection = forceDirection;
        this.forceDistance = forceDistance;
        this.timeToApplyForceOver = timeToApplyForceOver;
    }
}
