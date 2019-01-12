using UnityEngine;
using System;


[RequireComponent(typeof(MotionController))]
[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get { return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.05f, LayerMask.GetMask("Environment")); } }
    public float JumpHeight = 12f;
    public State CurrentState = State.Normal;

    public enum State { Normal, Hit, Attacking, Rolling }

    private PlayerAttackManager am;
    private MotionController mc;

    private InputData currentInputData;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;


    private void Awake()
    {
        am = GetComponent<PlayerAttackManager>();
        mc = GetComponent<MotionController>();
    }
    private void FixedUpdate()
    {
        mc.UpdateMotion();

        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        currentInputData = data;
        switch (CurrentState)
        {
            case State.Normal:
                CheckInputAsNormal();
                break;

            case State.Hit:
                break;

            case State.Attacking:
                break;

            case State.Rolling:
                break;

            default:
                throw new NotImplementedException("State " + CurrentState + " is not valid!");
        }
    }   
    public void SetState(State newState)
    {
        if (newState != CurrentState)
        {
            CurrentState = newState;
            Debug.Log(name + " changed state from " + CurrentState + " to " + newState);
        }
    }
    
    private void CheckInputAsNormal()
    {
        // Light attack button
        if (currentInputData.buttons[0])
        {
            am.NormalAttack();
        }

        // Jump
        if (currentInputData.axes[0] > 0.5 && IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.Impulse(new Vector2(0, JumpHeight));
        }
        // Walk
        if (currentInputData.axes[1] != 0)
        {
            mc.InputMotion = currentInputData.axes[1];
        }
    }
}