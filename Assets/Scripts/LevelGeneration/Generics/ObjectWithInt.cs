using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectWithInt<T> {

    public T objectToUse;
    public int value = 1;

    public ObjectWithInt(T objectToUse, int value)
    {
        this.objectToUse = objectToUse;
        this.value = value;
    }
}
