using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using Sierra.Combat2D;

[RequireComponent(typeof(EnemyAttackManager))]
public class DemonController : EnemyController
{
    #region Public Vars
    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Idle, Chasing, Throwing
    }

    public float ChaseRangeMax = 12;
    public float ChaseRangeMin = 8;
    public float ThrowRange = 10;
    public float MeleeRange = 4;

    [Serializable]
    public class DemonGizmoColours
    {
        public Color ThrowRange = Color.green;
        public Color ChaseRange = Color.cyan;
        public Color MeleeRange = Color.red;
    }
    public DemonGizmoColours GizmoColours;
    #endregion
    #region Protected Vars
    protected const float fastSpeed = 7.5f;
    protected const float slowSpeed = 2.5f;

    protected Hurtbox hurtbox;
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();
        hurtbox = GetComponent<Hurtbox>();
    }
    protected void OnDrawGizmosSelected()
    {
        DrawCircle(ChaseRangeMax, GizmoColours.ChaseRange);
        DrawCircle(ChaseRangeMin, GizmoColours.ChaseRange);
        DrawCircle(ThrowRange, GizmoColours.ThrowRange);
        DrawCircle(MeleeRange, GizmoColours.MeleeRange);
    }
    protected override void FixedUpdate()
    {
        if (CurrentState == State.Ready ||
            CurrentState == State.Action)
        {
            DecideAction();
            Act();
        }

        mc?.UpdatePosition();
    }
    #endregion

    #region Public Methods
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour != CurrentBehaviour)
            CurrentBehaviour = newBehaviour;
    }
    #endregion
    #region Protected Methods
    protected override void Act()
    {
        // Critical hurtbox on recovery
        if (am.AtkStage == AttackManager.AttackStage.Recovery)
        {
            hurtbox.SetState(Hurtbox.State.Critical);
        }
        else
        {
            hurtbox.SetState(Hurtbox.State.Armored);
        }

        switch (CurrentBehaviour)
        {
            case Behaviour.Idle:
                break;

            case Behaviour.Chasing:
                FacePlayer();

                if (mc == null) break;

                // Move
                if (PlayerToLeft)
                {
                    mc.MoveVector = Vector2.left;
                }
                else
                {
                    mc.MoveVector = Vector2.right;
                }
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
                if (DistToPlayer < ChaseRangeMax &&
                    DistToPlayer > ChaseRangeMin)
                    SetBehaviour(Behaviour.Chasing);
                break;

            case Behaviour.Chasing:
                if (DistToPlayer > ChaseRangeMax ||
                    DistToPlayer < ChaseRangeMin)
                {
                    SetBehaviour(Behaviour.Idle);
                    break;
                }

                if (DistToPlayer > ThrowRange)
                {
                    mc.XSpeed = fastSpeed;
                }
                else
                {
                    mc.XSpeed = slowSpeed;

                    // Start throw if close
                    if (CurrentState != State.Action) StartThrow();
                }

                break;

            default:
                break;
        }
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    protected void StartThrow()
    {
        SetState(State.Action);
        GenericEvents.OnAttack.Invoke();

        var dir = plr.transform.position - transform.position;
        am.RangedAttack(dir);
    }
    #endregion
}
