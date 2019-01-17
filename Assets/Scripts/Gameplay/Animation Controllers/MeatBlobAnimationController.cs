using UnityEngine;
using Spine.Unity;
using System;

public class MeatBlobAnimationController : EnemyAnimationController
{
    private MeatBlobController enm;

    private AnimState currentAnimationState = AnimState.Idle;
    private enum AnimState
    {
        Idle,
        Chasing,
        Hit,
        Dead
    }

    protected override void Awake()
    {
        base.Awake();
        enm = GetComponent<MeatBlobController>();
    }
    private void Start()
    {
        OrientTo(-1);
    }
    private void FixedUpdate()
    {
        RunAnimationStateMachine();
    }

    private void RunAnimationStateMachine()
    {
        if (enm.CurrentState == BaseController.State.Dead) ChangeToDeathState();
        if (enm.CurrentState == BaseController.State.InHitstun) ChangeToHitState();
    
        switch (currentAnimationState)
        {
            case AnimState.Idle:
                if (enm.CurrentBehaviour == EnemyController.Behaviour.Chasing) ChangeToChaseState();
                break;

            case AnimState.Chasing:
                if (enm.CurrentBehaviour == EnemyController.Behaviour.Idle) ChangeToIdleState();
                break;

            case AnimState.Hit:
                if (enm.CurrentState != BaseController.State.InHitstun) ChangeToHitState();
                break;

            case AnimState.Dead:
                Debug.Log(name + " is playing dead");
                break;

            default:
                throw new NotImplementedException("This animation state is not yet configured");
        }
    }

    public void ChangeToIdleState()
    {
        SetAnimationState(AnimState.Idle);
        sk_an.AnimationState.SetAnimation(0, "Idle", true);
    }
    public void ChangeToChaseState()
    {
        SetAnimationState(AnimState.Chasing);
        sk_an.AnimationState.SetAnimation(0, "Attack", true);
    }
    public void ChangeToHitState()
    {
        SetAnimationState(AnimState.Hit);
        sk_an.AnimationState.SetAnimation(0, "HitStun", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
    public void ChangeToDeathState()
    {
        SetAnimationState(AnimState.Dead);
        sk_an.AnimationState.SetAnimation(1, "Death", false);
    }

    private void SetAnimationState(AnimState newAnimationState)
    {
        currentAnimationState = newAnimationState;
    }
}
