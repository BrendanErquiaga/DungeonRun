using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3WithInt : SerializableDictionary<Vector3,int> {

    public Vector3 vectorToUse;
    public int value;

    public Vector3WithInt(Vector3 vectorToUse, int value)
    {
        this.vectorToUse = vectorToUse;
        this.value = value;
    }
}
