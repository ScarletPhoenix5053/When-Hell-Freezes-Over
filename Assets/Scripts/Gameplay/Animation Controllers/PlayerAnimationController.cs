using UnityEngine;
using System;
using Spine;
using Spine.Unity;

public class PlayerAnimationController : AnimationController
{
    /// <summary>
    /// Returns the <see cref="TrackEntry"/> of the first track in spine animation. Null if nothing is playing.
    /// </summary>
    public TrackEntry CurrentTrack { get { return sk_an.AnimationState.GetCurrent(0); }}

    private SkeletonAnimation sk_an;
    private PlayerController plr;
    private PlayerAttackManager am;
    private PlayerMotionController mc;

    private Color defaultColour;
    private PlayerAttackManager.AttackState previousAttackState = PlayerAttackManager.AttackState.None;

    private AnimState currentAnimationState = AnimState.Idle;
    private enum AnimState
    {
        Idle,
        IdleLong,

        Run,
        Jump,
        Fall,
        Land,

        HitStun,

        Attacking,

        Rolling,
        Climbing
    }

    protected override void Awake()
    {
        base.Awake();

        plr = GetComponent<PlayerController>();
        am = GetComponent<PlayerAttackManager>();
        mc = GetComponent<PlayerMotionController>();
        sk_an = GetComponent<SkeletonAnimation>();
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.HitStopActive) sk_an.timeScale = 0;
        else sk_an.timeScale = 1;
    }

    public void RunAnimationStateMachine()
    {
        switch (currentAnimationState)
        {
            case AnimState.Idle:
                if (plr.CurrentState == BaseController.State.Ready)
                {
                    if (mc.MoveVector.x != 0) ChangeToRunState();
                    if (!mc.IsGrounded) ChangeToFallState();
                }
                else if (plr.CurrentState == BaseController.State.InAction)
                {
                    if (plr.CurrentAction == PlayerController.Action.Rolling) ChangeToRollState();
                    else if (plr.CurrentAction == PlayerController.Action.Attacking) ChangeToAttackState();
                }
                break;

            case AnimState.Run:
                if (plr.CurrentState == BaseController.State.Ready)
                {
                    if (mc.MoveVector.x == 0) ChangeToIdleState();
                    if (!mc.IsGrounded) ChangeToFallState();
                }
                else if (plr.CurrentState == BaseController.State.InAction)
                {
                    if (plr.CurrentAction == PlayerController.Action.Rolling) ChangeToRollState();
                    else if (plr.CurrentAction == PlayerController.Action.Attacking) ChangeToAttackState();
                }
                break;

            case AnimState.Fall:
                if (plr.CurrentState == BaseController.State.Ready)
                {
                    if (mc.IsGrounded && mc.MoveVector.x != 0) ChangeToRunState();
                    if (mc.IsGrounded) ChangeToIdleState();
                }
                else if (plr.CurrentState == BaseController.State.InAction)
                {
                    if (plr.CurrentAction == PlayerController.Action.Rolling) ChangeToRollState();
                }
                break;

            case AnimState.Rolling:
                if (plr.CurrentState != BaseController.State.InAction || 
                    plr.CurrentAction != PlayerController.Action.Rolling)
                    ChangeToIdleState();
                break;

            case AnimState.Attacking:
                if (plr.CurrentState != BaseController.State.InAction ||
                    plr.CurrentAction != PlayerController.Action.Attacking)
                    ChangeToIdleState();

                var newAttackState = am.AtkState;
                if (previousAttackState != newAttackState && newAttackState != PlayerAttackManager.AttackState.None)
                {
                    
                    switch (newAttackState)
                    {
                        case PlayerAttackManager.AttackState.N1:
                            sk_an.AnimationState.SetAnimation(0, "MaceString1", false);
                            break;

                        case PlayerAttackManager.AttackState.N2:
                            sk_an.AnimationState.SetAnimation(0, "MaceString2", false);
                            break;

                        case PlayerAttackManager.AttackState.N3:
                            sk_an.AnimationState.SetAnimation(0, "MaceString3", false);
                            break;

                        case PlayerAttackManager.AttackState.Ranged:
                            break;
                        default:
                            throw new NotImplementedException("This attack animation sub-state is not yet configured!"); ;
                    }
                }
                previousAttackState = newAttackState;
                break;

            default:
                throw new NotImplementedException("This animation state is not yet configured");
        }
    }


    /// <summary>
    /// Sets X orientation to match sign. Automatically processes sign.
    /// </summary>
    /// <param name="sign"></param>
    public void OrientTo(int sign)
    {
        sign = Math.Sign(sign);
        sk_an.Skeleton.ScaleX = sign;
    }
    
    public void ChangeToIdleState()
    {
        SetAnimationState(AnimState.Idle);
        sk_an.AnimationState.SetAnimation(0, "Idle", true);
    }
    public void ChangeToRunState()
    {
        SetAnimationState(AnimState.Run);
        sk_an.AnimationState.SetAnimation(0, "OnehandRun", true);
    }
    public void ChangeToFallState()
    {
        SetAnimationState(AnimState.Fall);
        sk_an.AnimationState.SetAnimation(0, "Jump", true);
    }
    public void ChangeToRollState()
    {
        SetAnimationState(AnimState.Rolling);
        sk_an.AnimationState.SetAnimation(0, "Roll", false);
    }
    public void ChangeToAttackState()
    {
        SetAnimationState(AnimState.Attacking);
    }

    private void SetAnimationState(AnimState newAnimationState)
    {
        currentAnimationState = newAnimationState;
    }
}
