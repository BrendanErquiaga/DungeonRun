using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAtFixedRate : Executor
{
    [SerializeField]
    private int initialDelay = 0;

    [SerializeField]
    private int repeatRate = 1;

    [SerializeField]
    private bool initOnStart = true;

    public int InitialDelay
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

    public int RepeatRate
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
    public virtual void InitExecuteAfterDelay(int repeatRate = 1, int initialDelay = 0)
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
