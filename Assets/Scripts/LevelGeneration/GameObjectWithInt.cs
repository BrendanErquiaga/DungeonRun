using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectWithInt {

    public GameObject objectToUse;
    public int value;

    public GameObjectWithInt(GameObject objectToUse, int value)
    {
        this.objectToUse = objectToUse;
        this.value = value;
    }
}
