using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMotor : MonoBehaviour
{
    [SerializeField]
    protected CharacterController creatureCharController;
    [SerializeField]
    protected Vector3 creatureGravity = new Vector3(0, -9.81f, 0);
    [SerializeField]
    protected float horizontalAccelerationSpeed;
    [SerializeField]
    protected float horizontalDecelerationSpeed;
    [SerializeField]
    protected float maxHorizontalSpeed;
    [SerializeField]
    protected float groundedInfluence = 1f;
    [SerializeField]
    protected float airInfluence = 0.75f;

    protected List<ForceOverTime> forcesToApply = new List<ForceOverTime>();
    protected float moveSpeedOverallMultiplicativeModifier = 1;
    protected Vector3 velocity;
    protected bool acceptingMovementInput = true;

    #region Properties
    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }

        protected set
        {
            velocity = value;
        }
    }


    protected bool AcceptingMovementInput
    {
        get
        {
            return acceptingMovementInput;
        }

        set
        {
            acceptingMovementInput = value;
        }
    }

    protected Vector3 CreatureGravity
    {
        get
        {
            return creatureGravity;
        }

        set
        {
            creatureGravity = value;
        }
    }

    protected float HorizontalAccelerationSpeed
    {
        get
        {
            return horizontalAccelerationSpeed;
        }

        set
        {
            horizontalAccelerationSpeed = value;
        }
    }

    protected float HorizontalDecelerationSpeed
    {
        get
        {
            return horizontalDecelerationSpeed;
        }

        set
        {
            horizontalDecelerationSpeed = value;
        }
    }

    protected float MaxHorizontalSpeed
    {
        get
        {
            return maxHorizontalSpeed;
        }

        set
        {
            maxHorizontalSpeed = Mathf.Max(value, 0);
        }
    }

    protected float GroundedInfluence
    {
        get
        {
            return groundedInfluence;
        }

        set
        {
            groundedInfluence = Mathf.Max(value,0);
        }
    }

    protected float MoveSpeedOverallMultiplicativeModifier
    {
        get
        {
            return moveSpeedOverallMultiplicativeModifier;
        }

        set
        {
            moveSpeedOverallMultiplicativeModifier = Mathf.Max(value, 0);
        }
    }

    protected CharacterController CreatureCharController
    {
        get
        {
            if(creatureCharController == null)
            {
                creatureCharController = this.GetComponent<CharacterController>();
            }

            return creatureCharController;
        }

        set
        {
            creatureCharController = value;
        }
    }

    protected float AirInfluence
    {
        get
        {
            return airInfluence;
        }

        set
        {
            airInfluence = Mathf.Max(value, 0);
        }
    }
    #endregion

    #region Protected Methods
    protected virtual void Start()
    {
        CreatureMotorStart();
    }

    protected virtual void CreatureMotorStart() { }

    protected virtual void ProcessMotionInput(Vector2 moveInput)
    {
        UpdateVelocity(moveInput);
        ClampVelocity();
        TurnCreature();
    }

    protected virtual void MoveCreature()
    {
        CreatureCharController.Move(Velocity * Time.deltaTime);
    }

    protected virtual void ApplyForces()
    {
        Velocity = Velocity.SetX(Velocity.x * MoveSpeedOverallMultiplicativeModifier);
        Velocity = Velocity.SetZ(Velocity.z * MoveSpeedOverallMultiplicativeModifier);
        UpdateVelocityFromForces();
    }

    protected virtual void ApplyGravity()
    {
        if(!CreatureCharController.isGrounded)
        {
            Velocity += CreatureGravity * Time.deltaTime;
        } else if(Velocity.y < 0)
        {
            Velocity = Velocity.SetY(0);
        }
    }

    protected void UpdateVelocityFromForces()
    {
        for (int index = forcesToApply.Count - 1; index >= 0; index--)
        {
            Vector3 forceToAddThisFrame = Time.deltaTime / forcesToApply[index].timeToApplyForceOver * forcesToApply[index].forceDistance * forcesToApply[index].forceDirection;
            Velocity += forceToAddThisFrame;
            forcesToApply[index].timeElapsed += Time.deltaTime;
            if (forcesToApply[index].timeElapsed >= forcesToApply[index].timeToApplyForceOver)
                forcesToApply.RemoveAt(index);
        }
    }

    protected virtual void TurnCreature()
    {
        if (moveSpeedOverallMultiplicativeModifier > 0)
        {
            if (Velocity.XZ().normalized != Vector3.zero)
                CreatureCharController.transform.forward = Velocity.XZ().normalized;
        }
    }

    protected virtual void ClampVelocity()
    {
        float currentHorizontalSpeed = Velocity.XZ().magnitude;
        float currentMaxSpeed = MaxHorizontalSpeed;

        currentHorizontalSpeed = Mathf.Min(currentHorizontalSpeed, currentMaxSpeed);
        Velocity = Velocity.XZ().normalized * currentHorizontalSpeed + Vector3.up * Velocity.y;
    }

    protected virtual void UpdateVelocity(Vector2 moveInput)
    {
        Vector3 motionToAdd = CalculateMoveDirection(moveInput);
        motionToAdd *= CalculateAcceleration() * GetMovementInfluence();
        motionToAdd *= Time.deltaTime;
        Velocity += motionToAdd;
    }

    protected virtual float CalculateAcceleration()
    {
        return HorizontalAccelerationSpeed + HorizontalDecelerationSpeed;
    }

    protected virtual float GetMovementInfluence()
    {
        if (CreatureCharController.isGrounded)
            return GroundedInfluence;
        else
            return AirInfluence;
    }

    protected virtual Vector3 CalculateMoveDirection(Vector2 moveInput)
    {
        return new Vector3(moveInput.x, 0, moveInput.y);
    }

    protected virtual void DecelerateCreature()
    {
        float influence = 1;
        if (!this.CreatureCharController.isGrounded)
            influence = AirInfluence;

        float horizontalSpeed = velocity.XZ().magnitude;
        horizontalSpeed -= HorizontalDecelerationSpeed * Time.deltaTime * influence;
        horizontalSpeed = Mathf.Max(horizontalSpeed, 0);
        Velocity = Velocity.XZ().normalized * horizontalSpeed + Vector3.up * Velocity.y;
    }
#endregion

    public virtual void ProcessMotion(Vector2 moveInput)
    {
        if (CreatureCharController == null || !CreatureCharController.enabled)
        {
            Debug.LogWarning("There is no characterController enabled on this, can't process motion.", this.gameObject);
            return;
        }

        DecelerateCreature();

        if (AcceptingMovementInput)
        {
            ProcessMotionInput(moveInput);
        }

        //Animate thing here
        ApplyGravity();
        ApplyForces();
        MoveCreature();
    }

    public void SetVelocityY(float newVelocityY)
    {
        Velocity = Velocity.SetY(newVelocityY);
    }

    public void AddForceOverTime(ForceOverTime forceToAdd)
    {
        forcesToApply.Add(forceToAdd);
    }
}
