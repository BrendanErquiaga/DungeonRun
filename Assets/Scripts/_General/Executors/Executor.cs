using UnityEngine;

public abstract class Executor : MonoBehaviour
{
    protected abstract void Execute();

    public abstract void PublicExecute();
}
