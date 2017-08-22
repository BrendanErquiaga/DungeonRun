using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{
    [SerializeField]
    protected bool activateOnStart = true;
    [SerializeField]
    protected float initialDelay = 0;
    [SerializeField]
    protected ProceduralObjectSpawner objectSpawner;

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if (activateOnStart)
        {
            Invoke("StartSpawningObjects", initialDelay);
        }
    }

    public virtual void StartSpawningObjects()
    {
    }
}
