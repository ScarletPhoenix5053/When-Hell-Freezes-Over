using UnityEngine;
using UnityEngine.Events;
using System;
using Spine.Unity;

[RequireComponent(typeof(Health))]
public abstract class EnemyController : BaseController
{
    public EnemyEvents GenericEvents;
    [Serializable]
    public class EnemyEvents
    {
        public UnityEvent OnMotionStart;
        public UnityEvent OnMotionEnd;
        public UnityEvent OnAttack;
    }

    protected PlayerController plr;
    protected Health hp;
    protected EnemyAttackManager am;

    protected float DistToPlayer { get { return Vector2.Distance(transform.position, plr.transform.position); } }
    protected bool PlayerToLeft { get { return plr.transform.position.x < transform.position.x; } }

    protected override void Awake()
    {
        base.Awake();

        plr = FindObjectOfType<PlayerController>();
        hp = GetComponent<Health>();
        am = GetComponent<EnemyAttackManager>();
    }
    protected virtual void FixedUpdate()
    {
        if (CurrentState == State.Ready)
        {
            DecideAction();
            Act();
        }

        mc?.UpdatePosition();
    }


    /// <summary>
    /// Perform death actions for this enemy.
    /// </summary>
    public override void Die()
    {
        Debug.Log(name + "Is Dead.");
        SetState(State.Dead);
        am?.StopAttack();

        // Despawn
        if (transform.parent != null) Destroy(transform.parent.gameObject, 1.2f);
        else Destroy(gameObject, 1.2f);
    }

    protected virtual void FacePlayer()
    {
        if (PlayerToLeft)
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
