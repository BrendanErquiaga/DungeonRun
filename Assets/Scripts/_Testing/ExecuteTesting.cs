using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTesting : ExecuteNTimes
{
    public ValueCounterChanger changer;

    public float valueToChange = 5;

    protected override void Execute()
    {
        base.Execute();
        changer.AddToValue(valueToChange);
        Debug.Log("Execute!");
    }
}
