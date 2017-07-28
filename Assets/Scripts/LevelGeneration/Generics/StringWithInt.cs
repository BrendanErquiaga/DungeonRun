using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringWithInt
{
    public string objectToUse;
    public int value = 1;

    public StringWithInt(string objectToUse, int value)
    {
        this.objectToUse = objectToUse;
        this.value = value;
    }
}
