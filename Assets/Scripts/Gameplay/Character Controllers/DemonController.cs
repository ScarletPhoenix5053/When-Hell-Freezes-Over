using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using Sierra;
using Sierra.Combat2D;

[RequireComponent(typeof(EnemyAttackManager))]
public class DemonController : EnemyController
{
    #region Public Vars
    public DemonEvents Events;
    [Serializable]
    public class DemonEvents
    {
        public UnityEvent OnThrowAttack;
    }

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Idle, Chasing, Evade
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

    public GameObject bone, demonChunk;//Put this in every enemy with what items they drop. Since we only have 3 enemies it's not bad.
    public Vector2 AverageItemVariance = new Vector2(3, 3);

    #endregion
    #region Protected Vars
    protected const float fastSpeed = 5f;
    protected const float slowSpeed = 2f;
    protected const float riseSpeed = 10f;
    protected const float minDistToGround = 10f;

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
            hurtbox.CurrentState = Hurtbox.State.Critical;
            DecideAction();
            Act();
        }

        mc?.UpdatePosition();
    }
    #endregion

    #region Public Methods
    public override void SetState(State newState)
    {
        base.SetState(newState);

        if (newState == State.SuperStun) mc.SetGravityEnabled();
        else mc.SetGravityEnabled(false);
    }
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
        
        // If close to ground
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * minDistToGround), Color.green, Time.fixedDeltaTime); 
        if (Physics2D.Raycast(
            transform.position,
            Vector2.down,
            minDistToGround,
            LayerMask.GetMask("Environment", "Platform")))
        {
            mc.MoveVector = Vector2.up;
            mc.YSpeed = riseSpeed;
        }
        else
        {
            mc.YSpeed = 0;
            mc.ContMotionVector.y = 0;
        }

        // If evading or chasing
        if (CurrentBehaviour == Behaviour.Chasing ||
            CurrentBehaviour == Behaviour.Evade)
        {

            // If in melee range
            if (DistToPlayer < MeleeRange)
            {
                if (CurrentState != State.Action) StartMelee();
            }

            // If in throw range
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
                    mc.MoveVector += Vector2.left;
                }
                else
                {
                    mc.MoveVector += Vector2.right;
                }
                break;

            case Behaviour.Evade:
                FacePlayer();

                if (mc == null) break;

                // Move
                if (PlayerToLeft)
                {
                    mc.MoveVector += Vector2.right;
                }
                else
                {
                    mc.MoveVector += Vector2.left;
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
                if (DistToPlayer > ChaseRangeMax)
                {
                    SetBehaviour(Behaviour.Idle);
                    break;
                }
                else if (DistToPlayer < ChaseRangeMin)
                {
                    SetBehaviour(Behaviour.Evade);
                    break;
                }
                break;

            case Behaviour.Evade:
                if (DistToPlayer > ChaseRangeMin)
                {
                    SetBehaviour(Behaviour.Chasing);
                    break;
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
        Events.OnThrowAttack.Invoke();

        var dir = plr.transform.position - transform.position;
        am.RangedAttack(dir);
    }
    protected void StartMelee()
    {
        SetState(State.Action);
        GenericEvents.OnAttack.Invoke();

        am.Attack();
    }

    public override void Die()
    {
        int lootNum = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < lootNum; i++)
        {
            var boneVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var boneVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var chunkVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var chunkVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var boneSpawnPos = new Vector3(
                transform.position.x + boneVarianceX,
                transform.position.y + boneVarianceY,
                transform.position.z);

            var eyeSpawnPos = new Vector3(
                transform.position.x + chunkVarianceX,
                transform.position.y + chunkVarianceY,
                transform.position.z);

            Instantiate(bone, boneSpawnPos, transform.rotation);
            Instantiate(demonChunk, eyeSpawnPos, transform.rotation);
        }

        base.Die();
    }
    #endregion
}
