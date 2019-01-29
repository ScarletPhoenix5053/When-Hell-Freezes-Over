using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Sierra;
using Sierra.Combat2D;


public class MeatblobController : EnemyController
{
    public float DetectionRange = 12f;
    public float MinimumAttackRange = 2f;
    public float MaximumAttackRange = 5f;

    [Range(0,180)]
    public int WindupFrames = 60;
    [Range(0,300)]
    public int RecoveryFrames = 180;

    public Vector2 AverageLeapVelocity = new Vector2(10, 10);
    public Vector2 AverageLeapVariance = new Vector2(5, 5);

    [Range(0, 1)]
    public float EnterWindupChance = 0.1f;
    [Range(0, 1)]
    public float LeapChance = 0.1f;

    public Behaviour CurrentBehaviour = Behaviour.Idle;
    public enum Behaviour { Idle, Chasing, WindingUp, Leaping, Recovering }

    protected IEnumerator currentRoutine;

    protected int leapFrameBuffer = 5;
    protected int leapFrameCurrent = 0;

    protected void OnDrawGizmos()
    {
        DrawStateGizmo();
    }
    protected void OnDrawGizmosSelected()
    {
        DrawCircle(DetectionRange, Color.cyan);
        DrawCircle(MaximumAttackRange, Color.green);
        DrawCircle(MinimumAttackRange, Color.red);
    }

    /// <summary>
    /// Change enemy's behaviour state
    /// </summary>
    /// <param name="newBehaviour"></param>
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour == CurrentBehaviour) return;

        //Debug.Log(name + " changed behaviour state from " + currentBehaviour + " to " + newBehaviour);
        CurrentBehaviour = newBehaviour;
    }

    protected override void Act()
    {
        if (CurrentBehaviour != Behaviour.Leaping &&
            CurrentBehaviour != Behaviour.Recovering)
        {
            FacePlayer();
        }
        
        switch (CurrentBehaviour)
        {
            case Behaviour.Chasing:
                var dir = 0;
                if (PlayerToLeft) dir = -1; else dir = 1;
                mc.MoveVector = new Vector2(dir, 0);
                break;

            case Behaviour.Leaping:
                
                break;

            default:
                break;
        }
    }
    protected override void DecideAction()
    {
        switch (CurrentBehaviour)
        {
            case Behaviour.Idle:
                if (DistToPlayer < DetectionRange) SetBehaviour(Behaviour.Chasing);
                break;

            case Behaviour.Chasing:
                if ((DistToPlayer <= MinimumAttackRange) ||
                    (DistToPlayer <= MaximumAttackRange && Utility.GetRandomFloat() < EnterWindupChance)
                    )
                    StartWindup();
                break;
                
            case Behaviour.Leaping:
                if (mc.IsGrounded && leapFrameCurrent >= leapFrameBuffer)
                {
                    leapFrameCurrent = 0;
                    StartRecovery();
                }
                if (leapFrameCurrent < leapFrameBuffer) leapFrameCurrent++; 
                break;

            default:
                break;
        }
    }

    protected void StartWindup()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = WindupRoutine();
        StartCoroutine(currentRoutine);

        SetBehaviour(Behaviour.WindingUp);
    }
    protected void StartLeap()
    {
        SetBehaviour(Behaviour.Leaping);
        am.DoAttack(0);

        var xVariance = AverageLeapVariance.x * Utility.GetRandomFloat();
        var yVariance = AverageLeapVariance.y * Utility.GetRandomFloat();
        if (Utility.GetRandomFloat() < 0.5f) xVariance *= -1;
        if (Utility.GetRandomFloat() < 0.5f) yVariance *= -1;
        
        mc.DoImpulse(new Vector2((AverageLeapVelocity.x + xVariance) * Sign, AverageLeapVelocity.y + yVariance));
    }
    protected void StartRecovery()
    {
        SetBehaviour(Behaviour.Recovering);
        hp.Hurtbox.SetState(Hurtbox.State.Critical);
        am.StopAttack();

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = RecoveryRoutine();
        StartCoroutine(currentRoutine);
    }

    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour; 
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    protected void DrawStateGizmo()
    {
        var gizmoName = "Idle";

        if (CurrentState == State.HitStun)
        {
            gizmoName = "Stun";
        }
        else if (CurrentState == State.SuperStun)
        {
            gizmoName = "SuperStun";
        }
        else
        {
            switch (CurrentBehaviour)
            {
                case Behaviour.Chasing:
                    gizmoName = "Chasing";
                    break;

                case Behaviour.WindingUp:
                    gizmoName = "WindingUp";
                    break;

                case Behaviour.Leaping:
                    gizmoName = "Attacking";
                    break;

                case Behaviour.Recovering:
                    gizmoName = "Recovery";
                    break;

                default:
                    break;
            }
        }
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, gizmoName);
    }

    protected IEnumerator WindupRoutine()
    {
        var windupTimer = 0;
        yield return Utility.FrameTimer(WindupFrames, windupTimer);
        
        StartLeap();
    }
    protected IEnumerator RecoveryRoutine()
    {
        var recoveryTimer = 0;
        yield return Utility.FrameTimer(RecoveryFrames, recoveryTimer);

        SetBehaviour(Behaviour.Idle);
        hp.Hurtbox.SetState(Hurtbox.State.Vulnerable);
    }
}
