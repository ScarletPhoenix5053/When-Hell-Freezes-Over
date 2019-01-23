using UnityEngine;
using System;

public class LEGACYShieldMeatBlobAnimationController : EnemyAnimationController
{
    private LEGACYMeatBlobShieldedController enm;

    private AnimState currentAnimationState = AnimState.Idle;
    private enum AnimState
    {
        Idle,
        Hit,
        Dead
    }

    protected override void Awake()
    {
        base.Awake();
        enm = GetComponent<LEGACYMeatBlobShieldedController>();
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
        if (enm.CurrentState == BaseController.State.HitStun) ChangeToHitState();
        if (currentAnimationState == AnimState.Hit && enm.CurrentState != BaseController.State.HitStun) ChangeToIdleState();
    }

    public void ChangeToIdleState()
    {
        SetAnimationState(AnimState.Idle);
        sk_an.AnimationState.SetAnimation(0, "Idle", true);
    }
    public void ChangeToHitState()
    {
        SetAnimationState(AnimState.Hit);
        sk_an.AnimationState.SetAnimation(0, "HitStun", true);
    }
    public void ChangeToDeathState()
    {
        SetAnimationState(AnimState.Dead);
        sk_an.AnimationState.SetAnimation(0, "Death", false);
    }

    private void SetAnimationState(AnimState newAnimationState)
    {
        currentAnimationState = newAnimationState;
    }
}
