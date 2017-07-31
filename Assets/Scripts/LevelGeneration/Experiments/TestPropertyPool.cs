using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPropertyPool : PropertyPool
{
    int count = -1;
    public override string GetRandomObjectFromPool()
    {
        count++;
        return count.ToString();
    }
}
