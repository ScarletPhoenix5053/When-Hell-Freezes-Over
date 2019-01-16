using UnityEngine;
using System.Collections;

/// <summary>
/// Core Controller component for blob type enemy.
/// </summary>
public class BlobController : EnemyController
{
    public float AttackRange = 2f;
    public float ChaseRange = 10f;
    public float LeapHeight = 10f;
    public bool HoldState = false;

    protected BlobAnimationController an;
    protected BlobAttackManager am;

    protected override void Awake()
    {
        base.Awake();
        an = GetComponent<BlobAnimationController>();
        am = GetComponent<BlobAttackManager>();
    }
    protected virtual void LateUpdate()
    {
        FacePlayer();
    }

    public override void SetState(State newState)
    {
        base.SetState(newState);
        if (newState == State.InHitstun)
        {
            an.PlayHitStun();
            am.StopAttack();
        }
    }
    protected override void DecideAction()
    {
        if (HoldState) return;

        // if player doesn't exist or is too far away
        if (plr == null || distToPlayer > ChaseRange)
        {
            SetBehaviour(Behaviour.Idle);
            return;
        }
        
        // if attack in progress, stay attacking
        if (am.Attacking)
        {
            SetBehaviour(Behaviour.Attacking);
            return;
        }

        // If player is close enough
        if (distToPlayer < AttackRange)
        {
            // Attack
            SetBehaviour(Behaviour.Attacking);
        }
        // Else
        else
        {
            // Move closer
            SetBehaviour(Behaviour.Chasing);
        }        
    }
    protected override void Act()
    {
        if (CurrentState == State.Dead) return;

        if (CurrentState == State.Ready)
        {
            switch (CurrentBehaviour)
            {
                case Behaviour.Idle:
                    an.StopHitStun();
                    break;

                case Behaviour.Attacking:
                    if (!am.Attacking)
                    {
                        an.PlayAttack();
                        am.Attack();
                        StartCoroutine(ApplyLeapVelocity());
                    }
                    break;

                case Behaviour.Chasing:
                    an.StopHitStun();
                    if (playerToLeft)
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
    }

    private IEnumerator ApplyLeapVelocity()
    {
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(am.Attacks[0].Startup));

        mc.DoImpulse(new Vector2(0, LeapHeight));

        var frameCount = am.Attacks[0].Startup;
    }
}