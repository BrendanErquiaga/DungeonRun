using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptReferencer<T> : MonoBehaviour
{
    [SerializeField]
    protected T scriptReference;

    public T ScriptReference
    {
        get
        {
            return scriptReference;
        }

        protected set
        {
            scriptReference = value;
        }
    }
}
