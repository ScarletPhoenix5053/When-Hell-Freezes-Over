using UnityEngine;
using System;
using Sierra.Combat2D;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(EnemyAttackManager))]
[RequireComponent(typeof(CharacterMotionController))]
[RequireComponent(typeof(Health))]
public abstract class EnemyController : BaseController
{
    public Behaviour CurrentBehaviour = Behaviour.Idle;
    public enum Behaviour { Idle, Attacking, Chasing }

    protected PlayerController plr;
    protected Health hp;

    protected float distToPlayer { get { return Vector2.Distance(transform.position, plr.transform.position); } }
    protected bool playerToLeft { get { return plr.transform.position.x < transform.position.x; } }

    protected override void Awake()
    {
        base.Awake();
        plr = FindObjectOfType<PlayerController>();
        hp = GetComponent<Health>();
    }
    protected virtual void FixedUpdate()
    {
        if (CurrentState == State.Ready || CurrentState == State.InAction)
        {
            DecideAction();
            Act();
        }

        mc.UpdatePosition();
    }

    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour == CurrentBehaviour) return;

        //Debug.Log(name + " changed behaviour state from " + currentBehaviour + " to " + newBehaviour);
        CurrentBehaviour = newBehaviour;
    }
    /// <summary>
    /// Perform death actions for this enemy.
    /// </summary>
    public override void Die()
    {
        Debug.Log(name + "Is Dead.");
        SetState(State.Dead);
        StopAllCoroutines();

        // Death anim
        GetComponent<EnemyAnimationController>().PlayDeath();

        // Deactivate hurtbox
        foreach (Hurtbox hurtbox in hp.Hurtboxes)
        {
            hurtbox.SetInactive();
        }

        // Despawn after delay
        Destroy(gameObject, 3f);
    }

    protected void FacePlayer()
    {
        if (playerToLeft)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
    }
    protected abstract void DecideAction();
    protected abstract void Act();
}
