using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTesting : MonoBehaviour
{

    public InputManager inputManager;

    public Light lightToAdjust;

    private void Start()
    {
        inputManager.Action1ButtonDown += InputManager_Action1ButtonDown;
        inputManager.ButtonDown += InputManager_ButtonDown;
    }

    private void InputManager_ButtonDown()
    {
        Debug.Log("Pressed a button!");
        lightToAdjust.color = new Color(getRandomNum(), getRandomNum(), getRandomNum());
    }

    private float getRandomNum()
    {
        return Random.Range(0, 256);
    }

    private void InputManager_Action1ButtonDown()
    {
        Debug.Log("Pressed the action button!");
    }
}
