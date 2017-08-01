using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToMotionConnector : MonoBehaviour
{
    [SerializeField]
    protected CreatureMotor creatureMotor;
    [SerializeField]
    protected InputManager inputManager;

    private void OnEnable()
    {
        if(inputManager == null || creatureMotor == null)
        {
            Debug.LogWarning("InputToMotionConnection no setup properly.");
            return;
        }

        inputManager.LeftAxisChanged += InputManager_LeftAxisChanged;
    }

    private void OnDisable()
    {
        if (inputManager == null || creatureMotor == null)
        {
            Debug.LogWarning("InputToMotionConnection no setup properly.");
            return;
        }

        inputManager.LeftAxisChanged -= InputManager_LeftAxisChanged;
    }

    private void InputManager_LeftAxisChanged()
    {
        //Debug.Log("Sending this to process: " + inputManager.LeftAxisInputVector);
        creatureMotor.ProcessMotion(inputManager.LeftAxisInputVector);
    }
}
