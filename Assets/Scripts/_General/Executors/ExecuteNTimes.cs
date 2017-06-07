using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteNTimes : ExecuteAtFixedRate
{
    [SerializeField]
    private int numberOfTimesToExecute = 3;

    private int executedCount = 0;

    public int NumberOfTimesToExecute
    {
        get
        {
            return numberOfTimesToExecute;
        }

        protected set
        {
            numberOfTimesToExecute = value;
        }
    }

    protected override void Execute()
    {
        base.Execute();

        executedCount++;
        if(executedCount == NumberOfTimesToExecute)
        {
            CancelInvoke("Execute");
        }
    }
}
