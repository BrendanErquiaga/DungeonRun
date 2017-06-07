using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTesting : ExecuteNTimes
{
    protected override void Execute()
    {
        base.Execute();
        Debug.Log("Execute!");
    }
}
