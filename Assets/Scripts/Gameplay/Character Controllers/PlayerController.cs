﻿using UnityEngine;
using System;
using System.Collections;
using Sierra.Combat2D;


[RequireComponent(typeof(CharacterMotionController))]
[RequireComponent(typeof(PlayerAttackManager))]
[RequireComponent(typeof(Health))]
public class PlayerController : BaseController
{
    public float JumpHeight = 12f;
    public int RollFrames = 45;    
    public Action CurrentAction = Action.None;
    public enum Action { None, Attacking, Rolling, Climbing }
    public int Sign { get { if (mc.MoveVector.x == 0) return sign; else return sign = Math.Sign(mc.MoveVector.x); } }

    public Canvas TempDeathCanvas;

    private PlayerAttackManager am;
    private PlayerAnimationController an;
    private Health hp;

    private InputData currentInputData;
    private IEnumerator currentRollRoutine;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;
    private int sign = 1;


    protected override void Awake()
    {
        base.Awake();
        am = GetComponent<PlayerAttackManager>();
        an = GetComponent<PlayerAnimationController>();
        hp = GetComponent<Health>();

        if (TempDeathCanvas == null)
            throw new NullReferenceException("Please assign an object to TempDeathCanvas");
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.HitStopActive) return;
        OrientByMotion();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.HitStopActive) return;
        if (CurrentAction == Action.Rolling) mc.MoveVector = new Vector2(Math.Sign(transform.localScale.x), 0);

        IncrimentJumpTimer();
        mc.UpdatePosition();
    }


    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        currentInputData = data;
        if (CurrentState == State.Ready && CurrentAction == Action.None) CheckInputAsNormal();
    }
    /// <summary>
    /// Perform death actions for this character.
    /// </summary>
    public override void Die()
    {
        SetState(State.Dead);

        // death anim
        an.PlayDeath();

        // deactivate hurtbox
        foreach (Hurtbox hurtbox in GetComponent<Health>().Hurtboxes)
        {
            hurtbox.SetInactive();
        }

        // display you died message
        TempDeathCanvas.gameObject.SetActive(true);

        // restart game after delay
        GameManager.Instance.ReloadGame(3f);
    }
    public void SetAction(Action newAction)
    {
        if (CurrentAction == newAction) return;

        //Debug.Log("Changing player action from " + CurrentAction + " to " + newAction);
        CurrentAction = newAction;
    }

    /// <summary>
    /// Makes the player face x input direction
    /// </summary>
    private void OrientByMotion()
    {
        transform.localScale = new Vector3(Sign, transform.localScale.y, transform.localScale.z);
    }
    /// <summary>
    /// Performs input checks as if the character is unaffected by anything.
    /// </summary>
    private void CheckInputAsNormal()
    {
        // Dodge roll
        if (currentInputData.buttons[2])
        {
            if (currentRollRoutine != null) StopCoroutine(currentRollRoutine);
            currentRollRoutine = RollRoutine();
            StartCoroutine(currentRollRoutine);
        }
        // Ranged attack button
        else if (currentInputData.buttons[1])
        {
            am.RangedAttack();
        }
        // Light attack button
        else if (currentInputData.buttons[0])
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
    private IEnumerator RollRoutine()
    {
        SetAction(Action.Rolling);
        an.PlayDodgeRoll();
        foreach (Hurtbox hurtbox in hp.Hurtboxes)
        {
            hurtbox.SetInactive();
        }
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(RollFrames));
        yield return GameManager.Instance.UntillHitStopInactive();

        SetAction(Action.None);
        an.PlayIdle();
        foreach (Hurtbox hurtbox in hp.Hurtboxes)
        {
            hurtbox.SetActive();
        }
    }
}