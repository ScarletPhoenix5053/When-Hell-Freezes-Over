using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Sierra;


public class MeatblobController : EnemyController
{
    public float DetectionRange = 12f;
    public float MinimumAttackRange = 2f;
    public float MaximumAttackRange = 5f;

    public int MinimumWindupFrames = 60;
    public int MaximumWindupFrames = 120;
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

    protected int leapFrameBuffer = 10;
    protected int leapFrameCurrent = 0;

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
                    leapFrameBuffer = 0;
                    StartRecovery();
                }
                if (leapFrameCurrent < leapFrameBuffer) leapFrameBuffer++; 
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
        Debug.Log("Leaping");
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
        Debug.Log("Recovering");
        SetBehaviour(Behaviour.Recovering);
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

    protected IEnumerator WindupRoutine()
    {
        var readyToLeap = false;
        var windupTimer = 0;

        // Minimum wait duration
        while (windupTimer < MinimumWindupFrames)
        {
            windupTimer++;
            yield return new WaitForFixedUpdate();
        }

        // Leap after random duration
        while (!readyToLeap && windupTimer < MaximumWindupFrames)
        {
            if (Utility.GetRandomFloat() < LeapChance) readyToLeap = true;

            windupTimer++;
            yield return new WaitForFixedUpdate();
        }

        // Begin Leap
        StartLeap();
    }
    protected IEnumerator RecoveryRoutine()
    {
        var recoveryTimer = 0;
        while (recoveryTimer < RecoveryFrames)
        {
            recoveryTimer++;
            yield return new WaitForFixedUpdate();
        }

        SetBehaviour(Behaviour.Idle);
     }
}
