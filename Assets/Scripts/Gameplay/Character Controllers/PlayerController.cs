using UnityEngine;
using System;


[RequireComponent(typeof(MotionController))]
[RequireComponent(typeof(PlayerAttackManager))]
[RequireComponent(typeof(Health))]
public class PlayerController : BaseController
{
    public float JumpHeight = 12f;
    public enum Action { Attacking, Rolling, Climbing }

    private PlayerAttackManager am;

    private InputData currentInputData;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;


    protected override void Awake()
    {
        base.Awake();
        am = GetComponent<PlayerAttackManager>();
    }
    private void LateUpdate()
    {
        OrientByMotion();
    }
    private void FixedUpdate()
    {
        mc.UpdatePosition();

        IncrimentJumpTimer();
    }


    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        currentInputData = data;
        if (CurrentState == State.Ready) CheckInputAsNormal();
    }
    /// <summary>
    /// Perform death actions for this character.
    /// </summary>
    public override void Die()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Makes the player face x input direction
    /// </summary>
    private void OrientByMotion()
    {
        if (mc.MoveVector.x != 0)
        {
            transform.localScale = new Vector3(Math.Sign(mc.MoveVector.x), transform.localScale.y, transform.localScale.z);
        }
    }
    /// <summary>
    /// Performs input checks as if the character is unaffected by anything.
    /// </summary>
    private void CheckInputAsNormal()
    {
        // Light attack button
        if (currentInputData.buttons[0])
        {
            am.NormalAttack();
        }

        // Jump
        if (currentInputData.axes[0] > 0.5 && mc.IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.DoImpulse(new Vector2(0, JumpHeight));
        }
        // Walk
        if (currentInputData.axes[1] != 0)
        {
            mc.MoveVector = new Vector2(currentInputData.axes[1], 0);
        }
    }
    private void IncrimentJumpTimer()
    {
        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }
}