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

    Vector2 leftAxisInputVecot;
    public Vector2 LeftAxisInputVector { get { return leftAxisInputVecot; } set { leftAxisInputVecot = value; } }
    Vector2 rightAxisInputVector;
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
    public event InputPressed ActionButtonHeld;
    public event InputPressed ActionButtonReleased;

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

    public event InputPressed ActionButtonDown;
    public event InputPressed ActionButtonHeld;
    public event InputPressed ActionButtonReleased;

    public event InputPressed NonActionButtonPressed;
    #endregion

    #region Event Invokers
    public virtual void OnActionButtonDown()
    {
        InputPressed handler = ActionButtonDown;
        if (handler != null)
            handler();
    }

    public virtual void OnActionButtonHeld()
    {
        InputPressed handler = ActionButtonHeld;
        if (handler != null)
            handler();
    }

    public virtual void OnActionButtonReleased()
    {
        InputPressed handler = ActionButtonReleased;
        if (handler != null)
            handler();
    }

    public virtual void OnAction2ButtonDown()
    {
        InputPressed handler = Action2ButtonDown;
        if (handler != null)
            handler();
    }

    public virtual void OnAction2ButtonHeld()
    {
        InputPressed handler = Action2ButtonHeld;
        if (handler != null)
            handler();
    }

    public virtual void OnSpecialButtonReleased()
    {
        InputPressed handler = Action2ButtonReleased;
        if (handler != null)
            handler();

        OnNonInteractButtonReleased();
    }

    public virtual void OnTileButtonDown()
    {
        InputPressed handler = Action3ButtonDown;
        if (handler != null)
            handler();

        OnNonInteractButtonDown();
    }

    public virtual void OnTileButtonHeld()
    {
        InputPressed handler = Action3ButtonHeld;
        if (handler != null)
            handler();

        OnNonInteractButtonHeld();
    }

    public virtual void OnTileButtonReleased()
    {
        InputPressed handler = Action3ButtonReleased;
        if (handler != null)
            handler();

        OnNonInteractButtonReleased();
    }

    public virtual void OnCameraLockButtonDown()
    {
        InputPressed handler = Action4ButtonDown;
        if (handler != null)
            handler();

        OnNonInteractButtonDown();
    }

    public virtual void OnCameraLockButtonHeld()
    {
        InputPressed handler = Action4ButtonHeld;
        if (handler != null)
            handler();

        OnNonInteractButtonHeld();
    }

    public virtual void OnCameraLockButtonReleased()
    {
        InputPressed handler = Action4ButtonReleased;
        if (handler != null)
            handler();

        OnNonInteractButtonReleased();
    }

    public virtual void OnAlertButtonDown()
    {
        InputPressed handler = PauseButtonDown;
        if (handler != null)
            handler();

        OnNonInteractButtonDown();
    }

    public virtual void OnAlertButtonHeld()
    {
        InputPressed handler = PauseButtonHeld;
        if (handler != null)
            handler();

        OnNonInteractButtonHeld();
    }

    public virtual void OnAlertButtonReleased()
    {
        InputPressed handler = PauseButtonReleased;
        if (handler != null)
            handler();

        OnNonInteractButtonReleased();
    }

    public virtual void OnSwapClassButtonDown()
    {
        InputPressed handler = SelectButtonDown;
        if (handler != null)
            handler();

        OnNonInteractButtonDown();
    }

    public virtual void OnSwapClassButtonHeld()
    {
        InputPressed handler = SelectButtonHeld;
        if (handler != null)
            handler();

        OnNonInteractButtonHeld();
    }

    public virtual void OnSwapClassButtonReleased()
    {
        InputPressed handler = SelectButtonReleased;
        if (handler != null)
            handler();

        OnNonInteractButtonReleased();
    }

    public virtual void OnMovementAxisChanged()
    {
        InputPressed handler = LeftAxisChanged;
        if (handler != null)
            handler();
    }

    public virtual void OnMovementAxisLeftDirectionPressed()
    {
        InputPressed handler = LeftAxisLeftDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnMovementAxisRightDirectionPressed()
    {
        InputPressed handler = LeftAxisRightDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnMovementAxisUpDirectionPressed()
    {
        InputPressed handler = LeftAxisUpDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnMovementAxisDownDirectionPressed()
    {
        InputPressed handler = LeftAxisDownDirectionPressed;
        if (handler != null)
            handler();
    }

    public virtual void OnCameraAxisChanged()
    {
        InputPressed handler = RightAxisChanged;
        if (handler != null)
            handler();
    }

    public virtual void OnStartButtonDown()
    {
        InputPressed handler = StartButtonDown;
        if (handler != null)
            handler();
    }

    public virtual void OnStartButtonHeld()
    {
        InputPressed handler = StartButtonHeld;
        if (handler != null)
            handler();
    }

    public virtual void OnStartButtonReleased()
    {
        InputPressed handler = StartButtonReleased;
        if (handler != null)
            handler();
    }

    public virtual void OnInteractButtonDown()
    {
        InputPressed handler = InteractButtonDown;
        if (handler != null)
            handler();
    }

    public virtual void OnInteractButtonHeld()
    {
        InputPressed handler = InteractButtonHeld;
        if (handler != null)
            handler();
    }

    public virtual void OnInteractButtonReleased()
    {
        InputPressed handler = InteractButtonReleased;
        if (handler != null)
            handler();
    }

    protected virtual void OnNonInteractButtonDown()
    {
        InputPressed handler = NonInteractButtonDown;
        if (handler != null)
            handler();
    }

    protected virtual void OnNonInteractButtonHeld()
    {
        InputPressed handler = NonInteractButtonHeld;
        if (handler != null)
            handler();
    }

    protected virtual void OnNonInteractButtonReleased()
    {
        InputPressed handler = NonInteractButtonReleased;
        if (handler != null)
            handler();
    }
    #endregion

    #region Methods
    void Update()
    {
        this.CheckInput();
    }

    private void CheckInput()
    {
        CheckAxisInput_PC();

        CheckActionInput_PC();
    }

    #region PC Axis Input
    private void CheckAxisInput_PC()
    {
        CheckMotionInput();
        CheckCameraMotionInput();
    }

    private void CheckMotionInput()
    {
        leftAxisInputVecot = new Vector2(Input.GetAxis("Horizontal" + playerID), Input.GetAxis("Vertical" + playerID));

        OnMovementAxisChanged();
        CheckMotionThreshold();
    }

    private void CheckCameraMotionInput()
    {
        cameraInputVector = new Vector2(Input.GetAxis("CameraStickHorizontal" + playerID), Input.GetAxis("CameraStickVertical" + playerID));

        OnCameraAxisChanged();
    }
    #endregion

    #region PC Action Input
    private void CheckActionInput_PC()
    {
        CheckSwapClassInput();

        CheckCameraLockInput();

        CheckSpecialInput();

        CheckTileAbilityInput();

        CheckAttackInput();

        CheckAlertInput();

        CheckStartInput();

        CheckInteractInput();
    }

    private void CheckSwapClassInput()
    {
        CheckGenericInputDown("SwapClass", SelectButtonDown, true);
        CheckGenericInputHeld("SwapClass", SelectButtonHeld, true);
        CheckGenericInputReleased("SwapClass", SelectButtonReleased, true);
    }

    private void CheckCameraLockInput()
    {
        CheckGenericInputDown("CameraLock", Action4ButtonDown, true);
        CheckGenericInputHeld("CameraLock", Action4ButtonHeld, true);
        CheckGenericInputReleased("CameraLock", Action4ButtonReleased, true);
    }

    private void CheckSpecialInput()
    {
        CheckGenericInputDown("Special", Action2ButtonDown, true);
        CheckGenericInputHeld("Special", Action2ButtonHeld, true);
        CheckGenericInputReleased("Special", Action2ButtonReleased, true);
    }

    private void CheckTileAbilityInput()
    {
        CheckGenericInputDown("TileAbility", Action3ButtonDown, true);
        CheckGenericInputHeld("TileAbility", Action3ButtonHeld, true);
        CheckGenericInputReleased("TileAbility", Action3ButtonReleased, true);
    }

    private void CheckAlertInput()
    {
        CheckGenericInputDown("Alert", PauseButtonDown);
        CheckGenericInputHeld("Alert", PauseButtonHeld);
        CheckGenericInputReleased("Alert", PauseButtonReleased);
    }

    private void CheckAttackInput()
    {
        CheckGenericInputDown("Attack", ActionButtonDown, true);
        CheckGenericInputHeld("Attack", ActionButtonHeld, true);
        CheckGenericInputReleased("Attack", ActionButtonReleased, true);
    }

    private void CheckStartInput()
    {
        CheckGenericInputDown("Menu", StartButtonDown);
        CheckGenericInputHeld("Menu", StartButtonHeld);
        CheckGenericInputReleased("Menu", StartButtonReleased);
    }

    private void CheckInteractInput()
    {
        CheckGenericInputDown("Interact", InteractButtonDown);
        CheckGenericInputHeld("Interact", InteractButtonHeld);
        CheckGenericInputReleased("Interact", InteractButtonReleased);
    }

    private void CheckGenericInputDown(string buttonName, InputPressed eventToCall, bool handleNonInteracButtonEvent = false)
    {
        if (Input.GetButtonDown(buttonName + playerID))
        {
            CallInputEvent(eventToCall);

            if (handleNonInteracButtonEvent)
                OnNonInteractButtonDown();
        }
    }

    private void CheckGenericInputHeld(string buttonName, InputPressed eventToCall, bool handleNonInteracButtonEvent = false)
    {
        if (Input.GetButton(buttonName + playerID))
        {
            CallInputEvent(eventToCall);

            if (handleNonInteracButtonEvent)
                OnNonInteractButtonHeld();
        }
    }

    private void CheckGenericInputReleased(string buttonName, InputPressed eventToCall, bool handleNonInteracButtonEvent = false)
    {
        if (Input.GetButtonUp(buttonName + playerID))
        {
            CallInputEvent(eventToCall);

            if (handleNonInteracButtonEvent)
                OnNonInteractButtonReleased();
        }
    }
    #endregion

    #region Generic Axis Functions
    private void CheckMotionThreshold()
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
                OnMovementAxisLeftDirectionPressed();
                break;
            case Direction.Right:
                OnMovementAxisRightDirectionPressed();
                break;
            case Direction.Up:
                OnMovementAxisUpDirectionPressed();
                break;
            case Direction.Down:
                OnMovementAxisDownDirectionPressed();
                break;
        }
    }

    private void CheckAxisForReset(Vector2 inputVector, ref Direction directionToCheck)
    {
        switch (directionToCheck)
        {
            case Direction.Left:
                if (inputVector.x > -AxisEdgeThreshold)
                    ResetLeftAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Right:
                if (inputVector.x < AxisEdgeThreshold)
                    ResetLeftAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Up:
                if (inputVector.y < AxisEdgeThreshold)
                    ResetLeftAxisDirectionPressed(ref directionToCheck);
                break;
            case Direction.Down:
                if (inputVector.y > -AxisEdgeThreshold)
                    ResetLeftAxisDirectionPressed(ref directionToCheck);
                break;
        }
    }

    private void ResetLeftAxisDirectionPressed(ref Direction directionToReset)
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
