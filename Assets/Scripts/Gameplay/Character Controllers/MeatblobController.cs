using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Sierra;
using Sierra.Combat2D;


public class MeatblobController : EnemyController
{
    #region Public Vars
    public MeatblobEvents BlobEvents;
    [Serializable]
    public class MeatblobEvents
    {
        public UnityEvent OnWindup;
        public UnityEvent OnLand;
    }
    public GameObject bone, eyeball;//Put this in every enemy with what items they drop. Since we only have 3 enemies it's not bad.
    public Vector2 AverageItemVariance = new Vector2(3, 3);

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
    #endregion
    #region Protected Vars
    protected IEnumerator currentRoutine;

    protected int leapFrameBuffer = 5;
    protected int leapFrameCurrent = 0;
    #endregion

    #region Unity Runtime Events
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
    #endregion

    #region Public Methods
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
    //Put this in all 3 types of enemies. 
    public override void Die()
    {
        int lootNum = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < lootNum; i++)
        {
            var boneVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var boneVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var eyeVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var eyeVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var boneSpawnPos = new Vector3(
                transform.position.x + boneVarianceX,
                transform.position.y + boneVarianceY,
                transform.position.z);

            var eyeSpawnPos = new Vector3(
                transform.position.x + eyeVarianceX,
                transform.position.y + eyeVarianceY,
                transform.position.z);

            Instantiate(bone, boneSpawnPos, transform.rotation);
            Instantiate(eyeball, eyeSpawnPos, transform.rotation);
        }

        if (currentRoutine != null) StopCoroutine(currentRoutine);

        base.Die();
    }
    #endregion
    #region Protected Methods
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
                if (DistToPlayer < DetectionRange)
                {
                    Debug.Log("chase");
                    StartChase();
                }
                else
                {
                }
                break;

            case Behaviour.Chasing:
                if ((DistToPlayer <= MinimumAttackRange) ||
                    (DistToPlayer <= MaximumAttackRange && Utility.GetRandomFloat() < EnterWindupChance)
                    )
                    StartWindup();

                if (DistToPlayer > DetectionRange)
                {
                    Debug.Log("wait");
                    StopChase();
                }
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

    protected void StopChase()
    {
        GenericEvents.OnMotionEnd.Invoke();
        SetBehaviour(Behaviour.Idle);
    }
    protected void StartChase()
    {
        GenericEvents.OnMotionStart.Invoke();
        SetBehaviour(Behaviour.Chasing);
    }
    protected void StartWindup()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = WindupRoutine();
        StartCoroutine(currentRoutine);

        GenericEvents.OnMotionEnd.Invoke();
        SetBehaviour(Behaviour.WindingUp);
    }
    protected void StartLeap()
    {
        GenericEvents.OnAttack.Invoke();
        SetBehaviour(Behaviour.Leaping);
        hp.Hurtbox.SetState(Hurtbox.State.Armored);
        am.DoAttack(0);

        var xVariance = AverageLeapVariance.x * Utility.GetRandomFloat();
        var yVariance = AverageLeapVariance.y * Utility.GetRandomFloat();
        if (Utility.GetRandomFloat() < 0.5f) xVariance *= -1;
        if (Utility.GetRandomFloat() < 0.5f) yVariance *= -1;
        
        mc.DoImpulse(new Vector2((AverageLeapVelocity.x + xVariance) * Sign, AverageLeapVelocity.y + yVariance));
    }
    protected void StartRecovery()
    {
        BlobEvents.OnLand.Invoke();
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
        hp.Hurtbox.SetState(Hurtbox.State.Armored);
    }
    #endregion
}
