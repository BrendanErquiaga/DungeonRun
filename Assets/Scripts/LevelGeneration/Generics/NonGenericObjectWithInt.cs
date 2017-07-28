using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NonGenericObjectWithInt : MonoBehaviour {

    public Object objectToUse;
    public int value = 1;

    public NonGenericObjectWithInt(Object objectToUse, int value)
    {
        this.objectToUse = objectToUse;
        this.value = value;
    }
}
