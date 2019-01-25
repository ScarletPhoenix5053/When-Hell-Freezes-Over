﻿using Sierra;
using Sierra.Combat2D;
using System.Collections;
using UnityEngine;

public class ShieldBlobController : EnemyController
{
    public float PushRange = 1f;
    public int TurnDuration = 120;
    public GameObject BouncePadPrefab;

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Guarding, ReadyToPush, Pushing
    }

    new protected EnemyAttackManager am;

    protected IEnumerator currentTurnRoutine;
    protected bool turning;
    protected int turningTo;

    protected override void Awake()
    {
        base.Awake();
        am = GetComponent<EnemyAttackManager>();
    }
    protected void OnDrawGizmosSelected()
    {
        Utility.DrawCircle(transform.position, PushRange, Color.red);
    }

    public void SetBehaviour(Behaviour newBehaviour)
    {
        CurrentBehaviour = newBehaviour;
    }

    protected override void Act()
    {
        // Turn to face player after delay on sideswitch
        if ((Sign == -1 && !PlayerToLeft) ||
            (Sign == 1 && PlayerToLeft))
            {
                if (!turning) StartCoroutine(TurnRoutine());
            }

        //  Block if facing player, be critical if not
        if ((Sign == -1 && PlayerToLeft) ||
            (Sign == 1 && !PlayerToLeft))
        {
            hp.Hurtbox.SetState(Hurtbox.State.Blocking);
        }
        else
        {
            hp.Hurtbox.SetState(Hurtbox.State.Critical);
        }


        // READYTOPUSH
        // Push player if stays close for too long
        if (CurrentBehaviour == Behaviour.ReadyToPush)
        {
            //am.Attack();
        }
    }
    protected override void DecideAction()
    {
        // Switch to push anticipation state when player is very close
        // Switch back to guarding if player is no longer close      

        switch (CurrentBehaviour)
        {
            case Behaviour.Guarding:
                if (DistToPlayer < PushRange) SetBehaviour(Behaviour.ReadyToPush);
                break;

            case Behaviour.ReadyToPush:
                if (DistToPlayer > PushRange) SetBehaviour(Behaviour.Guarding);
                break;

            default:
                break;
        }
    }
    public override void Die()
    {
        if (BouncePadPrefab != null)
            Instantiate(
                BouncePadPrefab,
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);
        base.Die();
    }

    protected IEnumerator TurnRoutine()
    {
        var turnTimer = 0;

        turning = true;
        yield return Utility.FrameTimer(TurnDuration, turnTimer);

        turning = false;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
