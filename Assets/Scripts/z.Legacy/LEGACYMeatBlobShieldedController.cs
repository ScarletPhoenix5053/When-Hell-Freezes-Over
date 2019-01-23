using UnityEngine;
using System;
using System.Collections;
using Sierra.Combat2D;

public class LEGACYMeatBlobShieldedController : EnemyController
{
    public int TurnDelayFrames = 60;
    public GameObject BouncePadPrefab;

    protected Hurtbox Hurtbox;

    protected bool waitingToTurn;
    protected bool guarding;
    protected int turnDelayTimer;

    protected bool facingLeft { get { return Math.Sign(transform.localScale.x) == -1; } }

    protected override void Awake()
    {
        base.Awake();
        Hurtbox = GetComponent<Hurtbox>();
    }

    protected override void Act()
    {
        // if facing left and player is left, guard
        if (facingLeft && playerToLeft)
        {
            Hurtbox.SetInactive();
        }
        // if facing right and player is right, guard
        else if (!facingLeft && !playerToLeft)
        {
            Hurtbox.SetInactive();
        }
        else
        {
            Hurtbox.SetActive();
        }
    }
    protected override void DecideAction()
    {
        // if facing left and player is on right, turn after delay
        // if facing right and player is on left, turn after delay
        if ((facingLeft && !playerToLeft) ||
            (!facingLeft && playerToLeft))
        {
            if (!waitingToTurn) StartCoroutine(TurnRoutine());
        }
    }

    public override void Die()
    {
        if (BouncePadPrefab != null)
            Instantiate(
                BouncePadPrefab,
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                Quaternion.identity);
        var eventRaiser = GetComponent<EventRaiser>();
        if (eventRaiser != null)
        {
            eventRaiser.RaiseEvent();
        }
        base.Die();
    }
    protected IEnumerator TurnRoutine()
    {
        waitingToTurn = true;
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(TurnDelayFrames));
        yield return GameManager.Instance.UntillHitStopInactive();
        while (CurrentState == State.HitStun) yield return null;
        
        waitingToTurn = false;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
