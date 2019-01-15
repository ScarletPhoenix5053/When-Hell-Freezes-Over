using UnityEngine;
using System.Collections;

public abstract class EnemyController : BaseController
{
    public Behaviour CurrentBehaviour = Behaviour.Idle;
    public enum Behaviour { Idle, Attacking, Chasing }

    protected PlayerController plr;

    protected float distToPlayer { get { return Vector2.Distance(transform.position, plr.transform.position); } }
    protected bool playerToLeft { get { return plr.transform.position.x < transform.position.x; } }

    protected override void Awake()
    {
        base.Awake();
        plr = FindObjectOfType<PlayerController>();
    }
    protected virtual void FixedUpdate()
    {
        if (CurrentState != State.InHitstun &&
            CurrentState != State.Dead)
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
