using UnityEngine;

public class ExecuteAfterDelay : Executor
{
    [SerializeField]
    private float delay;

    [SerializeField]
    private bool initOnStart = true;

    public float Delay
    {
        get
        {
            return delay;
        }

        protected set
        {
            delay = value;
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
            InitExecuteAfterDelay(Delay);
        }
    }

    /// <param name="delay">Delay in seconds</param>
    /// <param name="startDelayImmediately">If true starts the delay</param>
    public virtual void InitExecuteAfterDelay(float delay = 5, bool startDelayImmediately = true)
    {
        this.Delay = delay;

        if (startDelayImmediately)
        {
            StartDelay();
        }
    }

    public virtual void StartDelay()
    {
        Invoke("Execute", Delay);
    }

    public override void PublicExecute() { }

    protected override void Execute() { }
}
