using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Members
    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public enum ButtonType
    {
        Action,
        Action2,
        Action3,
        Action4,
        Pause,
        Select
    }

    [SerializeField]
    private float axisEdgeThreshold = 0.9f;
    Direction leftAxisLastDirectionPressed;
    Direction rightAxisLastDirectionPressed;

    Vector2 leftAxisInputVector;
    public Vector2 LeftAxisInputVector { get { return leftAxisInputVector; } set { leftAxisInputVector = value; } }
    public string leftAxisHorizontalName = "Horizontal";
    public string leftAxisVerticalName = "Vertical";
    Vector2 rightAxisInputVector;
    public string rightAxisHorizontalName = "CameraStickHorizontal";
    public string rightAxisVerticalName = "CameraStickVertical";
    public Vector2 RightAxisInputVector { get { return rightAxisInputVector; } set { rightAxisInputVector = value; } }

    protected float AxisEdgeThreshold
    {
        get
        {
            return axisEdgeThreshold;
        }

        set
        {
            axisEdgeThreshold = value;
        }
    }
    #endregion

    #region Input Delegates
    public delegate void InputPressed();
    public event InputPressed Action1ButtonDown;
    public event InputPressed Action1ButtonHeld;
    public event InputPressed Action1ButtonReleased;

    public event InputPressed Action2ButtonDown;
    public event InputPressed Action2ButtonHeld;
    public event InputPressed Action2ButtonReleased;

    public event InputPressed Action3ButtonDown;
    public event InputPressed Action3ButtonHeld;
    public event InputPressed Action3ButtonReleased;

    public event InputPressed Action4ButtonDown;
    public event InputPressed Action4ButtonHeld;
    public event InputPressed Action4ButtonReleased;

    public event InputPressed PauseButtonDown;
    public event InputPressed PauseButtonHeld;
    public event InputPressed PauseButtonReleased;

    public event InputPressed SelectButtonDown;
    public event InputPressed SelectButtonHeld;
    public event InputPressed SelectButtonReleased;

    public event InputPressed LeftAxisChanged;
    public event InputPressed LeftAxisLeftDirectionPressed;
    public event InputPressed LeftAxisRightDirectionPressed;
    public event InputPressed LeftAxisUpDirectionPressed;
    public event InputPressed LeftAxisDownDirectionPressed;

    public event InputPressed RightAxisChanged;
    public event InputPressed RightAxisLeftDirectionPressed;
    public event InputPressed RightAxisRightDirectionPressed;
    public event InputPressed RightAxisUpDirectionPressed;
    public event InputPressed RightAxisDownDirectionPressed;

    public event InputPressed ButtonDown;
    public event InputPressed ButtonHeld;
    public event InputPressed ButtonReleased;
    #endregion

    #region Event Invokers
    //public virtual void OnAction1ButtonDown()
    //{
    //    InputPressed handler = Action1ButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction1ButtonHeld()
    //{
    //    InputPressed handler = Action1ButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction1ButtonReleased()
    //{
    //    InputPressed handler = Action1ButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction2ButtonDown()
    //{
    //    InputPressed handler = Action2ButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction2ButtonHeld()
    //{
    //    InputPressed handler = Action2ButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction2ButtonReleased()
    //{
    //    InputPressed handler = Action2ButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction3ButtonDown()
    //{
    //    InputPressed handler = Action3ButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction3ButtonHeld()
    //{
    //    InputPressed handler = Action3ButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction3ButtonReleased()
    //{
    //    InputPressed handler = Action3ButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction4ButtonDown()
    //{
    //    InputPressed handler = Action4ButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction4ButtonHeld()
    //{
    //    InputPressed handler = Action4ButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnAction4ButtonReleased()
    //{
    //    InputPressed handler = Action4ButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnPauseButtonDown()
    //{
    //    InputPressed handler = PauseButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnPauseButtonHeld()
    //{
    //    InputPressed handler = PauseButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnPauseButtonReleased()
    //{
    //    InputPressed handler = PauseButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnSelectButtonDown()
    //{
    //    InputPressed handler = SelectButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnSelectButtonHeld()
    //{
    //    InputPressed handler = SelectButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnSelectButtonReleased()
    //{
    //    InputPressed handler = SelectButtonReleased;
    //    if (handler != null)
    //        handler();
    //}

    public virtual void OnLeftAxisChanged()
    {
        InputPressed handler = LeftAxisChanged;
        if (handler != null)
            handler();
    }

    public virtual void OnLeftAxisLeftDirectionPressed()
    {
        InputPressed handler = LeftAxisLeftDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnLeftAxisRightDirectionPressed()
    {
        InputPressed handler = LeftAxisRightDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnLeftAxisUpDirectionPressed()
    {
        InputPressed handler = LeftAxisUpDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnLeftAxisDownDirectionPressed()
    {
        InputPressed handler = LeftAxisDownDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnRightAxisChanged()
    {
        InputPressed handler = RightAxisChanged;
        if (handler != null)
            handler();
    }

    public virtual void OnRightAxisLeftDirectionPressed()
    {
        InputPressed handler = RightAxisLeftDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnRightAxisRightDirectionPressed()
    {
        InputPressed handler = RightAxisRightDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnRightAxisUpDirectionPressed()
    {
        InputPressed handler = RightAxisUpDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnRightAxisDownDirectionPressed()
    {
        InputPressed handler = RightAxisDownDirectionPressed;
        if (handler != null)
            handler();
    }

    //public virtual void OnButtonDown()
    //{
    //    InputPressed handler = ButtonDown;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnButtonHeld()
    //{
    //    InputPressed handler = ButtonHeld;
    //    if (handler != null)
    //        handler();
    //}

    //public virtual void OnButtonReleased()
    //{
    //    InputPressed handler = ButtonReleased;
    //    if (handler != null)
    //        handler();
    //}
    #endregion

    #region Methods
    void Update()
    {
        this.CheckInput();
    }

    private void CheckInput()
    {
        CheckAxisInput();

        CheckButtonInput();
    }

    #region Axis Input
    private void CheckAxisInput()
    {
        CheckLeftAxisInput();
        CheckRightAxisInput();
    }

    private void CheckLeftAxisInput()
    {
        leftAxisInputVector = new Vector2(Input.GetAxis(leftAxisHorizontalName), Input.GetAxis(leftAxisVerticalName));

        OnLeftAxisChanged();
        CheckLeftAxisDirection();
    }

    private void CheckRightAxisInput()
    {
        rightAxisInputVector = new Vector2(Input.GetAxis(rightAxisHorizontalName), Input.GetAxis(rightAxisVerticalName));

        OnRightAxisChanged();
        CheckRightAxisDirection();
    }
    #endregion

    #region Button Input
    private void CheckButtonInput()
    {
        CheckAction1ButtonInput();

        CheckAction2ButtonInput();

        CheckAction3ButtonInput();

        CheckAction4ButtonInput();

        CheckPauseButtonInput();

        CheckSelectButtonInput();
    }

    private void CheckAction1ButtonInput()
    {
        CheckGenericInputDown("Action1", Action1ButtonDown);
        CheckGenericInputHeld("Action1", Action1ButtonHeld);
        CheckGenericInputReleased("Action1", Action1ButtonReleased);
    }

    private void CheckAction2ButtonInput()
    {
        CheckGenericInputDown("Action2", Action2ButtonDown);
        CheckGenericInputHeld("Action2", Action2ButtonHeld);
        CheckGenericInputReleased("Action2", Action2ButtonReleased);
    }

    private void CheckAction3ButtonInput()
    {
        CheckGenericInputDown("Action3", Action3ButtonDown);
        CheckGenericInputHeld("Action3", Action3ButtonHeld);
        CheckGenericInputReleased("Action3", Action3ButtonReleased);
    }

    private void CheckAction4ButtonInput()
    {
        CheckGenericInputDown("Action4", Action4ButtonDown);
        CheckGenericInputHeld("Action4", Action4ButtonHeld);
        CheckGenericInputReleased("Action4", Action4ButtonReleased);
    }

    private void CheckPauseButtonInput()
    {
        CheckGenericInputDown("Pause", PauseButtonDown);
        CheckGenericInputHeld("Pause", PauseButtonHeld);
        CheckGenericInputReleased("Pause", PauseButtonReleased);
    }

    private void CheckSelectButtonInput()
    {
        CheckGenericInputDown("Select", SelectButtonDown);
        CheckGenericInputHeld("Select", SelectButtonHeld);
        CheckGenericInputReleased("Select", SelectButtonReleased);
    }

    private void CheckGenericInputDown(string buttonName, InputPressed eventToCall)
    {
        if (Input.GetButtonDown(buttonName))
        {
            CallInputEvent(eventToCall);

            CallInputEvent(ButtonDown);
        }
    }

    private void CheckGenericInputHeld(string buttonName, InputPressed eventToCall)
    {
        if (Input.GetButton(buttonName))
        {
            CallInputEvent(eventToCall);

            CallInputEvent(ButtonHeld);
        }
    }

    private void CheckGenericInputReleased(string buttonName, InputPressed eventToCall)
    {
        if (Input.GetButtonUp(buttonName))
        {
            CallInputEvent(eventToCall);

            CallInputEvent(ButtonReleased);
        }
    }
    #endregion

    #region Generic Axis Functions
    private void CheckLeftAxisDirection()
    {
        if (leftAxisLastDirectionPressed != Direction.None)
            CheckAxisForReset(LeftAxisInputVector, ref leftAxisLastDirectionPressed);
        else
        {
            if (LeftAxisInputVector.x < -AxisEdgeThreshold)
                ChangeLeftAxisDirectionPressed(Direction.Left);
            else if (LeftAxisInputVector.x > AxisEdgeThreshold)
                ChangeLeftAxisDirectionPressed(Direction.Right);
            else if (LeftAxisInputVector.y > AxisEdgeThreshold)
                ChangeLeftAxisDirectionPressed(Direction.Up);
            else if (LeftAxisInputVector.y < -AxisEdgeThreshold)
                ChangeLeftAxisDirectionPressed(Direction.Down);
        }
    }

    private void ChangeLeftAxisDirectionPressed(Direction directionPressed)
    {
        leftAxisLastDirectionPressed = directionPressed;

        FireLeftAxisDirectionEvent();
    }

    private void FireLeftAxisDirectionEvent()
    {
        switch (leftAxisLastDirectionPressed)
        {
            case Direction.Left:
                OnLeftAxisLeftDirectionPressed();
                break;
            case Direction.Right:
                OnLeftAxisRightDirectionPressed();
                break;
            case Direction.Up:
                OnLeftAxisUpDirectionPressed();
                break;
            case Direction.Down:
                OnLeftAxisDownDirectionPressed();
                break;
        }
    }

    private void CheckRightAxisDirection()
    {
        if (rightAxisLastDirectionPressed != Direction.None)
            CheckAxisForReset(RightAxisInputVector, ref rightAxisLastDirectionPressed);
        else
        {
            if (RightAxisInputVector.x < -AxisEdgeThreshold)
                ChangeRightAxisDirectionPressed(Direction.Left);
            else if (RightAxisInputVector.x > AxisEdgeThreshold)
                ChangeRightAxisDirectionPressed(Direction.Right);
            else if (RightAxisInputVector.y > AxisEdgeThreshold)
                ChangeRightAxisDirectionPressed(Direction.Up);
            else if (RightAxisInputVector.y < -AxisEdgeThreshold)
                ChangeRightAxisDirectionPressed(Direction.Down);
        }
    }

    private void ChangeRightAxisDirectionPressed(Direction directionPressed)
    {
        rightAxisLastDirectionPressed = directionPressed;

        FireRightAxisDirectionEvent();
    }

    private void FireRightAxisDirectionEvent()
    {
        switch (rightAxisLastDirectionPressed)
        {
            case Direction.Left:
                OnRightAxisLeftDirectionPressed();
                break;
            case Direction.Right:
                OnRightAxisRightDirectionPressed();
                break;
            case Direction.Up:
                OnRightAxisUpDirectionPressed();
                break;
            case Direction.Down:
                OnRightAxisDownDirectionPressed();
                break;
        }
    }

    private void CheckAxisForReset(Vector2 inputVector, ref Direction directionToCheck)
    {
        switch (directionToCheck)
        {
            case Direction.Left:
                if (inputVector.x > -AxisEdgeThreshold)
                    ResetAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Right:
                if (inputVector.x < AxisEdgeThreshold)
                    ResetAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Up:
                if (inputVector.y < AxisEdgeThreshold)
                    ResetAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Down:
                if (inputVector.y > -AxisEdgeThreshold)
                    ResetAxisDirectionPressed(ref directionToCheck);
                break;
        }
    }

    private void ResetAxisDirectionPressed(ref Direction directionToReset)
    {
        directionToReset = Direction.None;
    }
    #endregion

    public void CallInputEvent(InputPressed eventToCall)
    {
        if (eventToCall != null)
            eventToCall();
    }
    #endregion
}
