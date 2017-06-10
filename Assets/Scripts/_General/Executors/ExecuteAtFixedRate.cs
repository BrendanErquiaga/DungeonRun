using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAtFixedRate : Executor
{
    [SerializeField]
    private float initialDelay = 0;

    [SerializeField]
    private float repeatRate = 1;

    [SerializeField]
    private bool initOnStart = true;

    public float InitialDelay
    {
        get
        {
            return initialDelay;
        }

        protected set
        {
            initialDelay = value;
        }
    }

    public float RepeatRate
    {
        get
        {
            return repeatRate;
        }

        protected set
        {
            repeatRate = value;
        }
    }

    public bool InitOnStart
    {
        get
        {
            return initOnStart;
        }

        protected set
        {
            initOnStart = value;
        }
    }

    private void Start()
    {
        if (InitOnStart)
        {
            InitExecuteAfterDelay(RepeatRate, InitialDelay);
        }
    }

    /// <param name="repeatRate">Rate in seconds</param>
    /// <param name="startDelayImmediately">If true starts the delay</param>
    public virtual void InitExecuteAfterDelay(float repeatRate = 1, float initialDelay = 0)
    {
        this.RepeatRate = repeatRate;
        this.InitialDelay = initialDelay; 

        StartExecuting();
    }

    public virtual void StartExecuting()
    {
        InvokeRepeating("Execute", InitialDelay, RepeatRate);
    }

    public override void PublicExecute() { }

    protected override void Execute() { }
}
