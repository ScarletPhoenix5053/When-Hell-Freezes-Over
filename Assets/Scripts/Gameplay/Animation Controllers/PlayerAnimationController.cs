using UnityEngine;
using System;
using Spine;
using Spine.Unity;

public class PlayerAnimationController : AnimationController
{
    public bool CloseToGround
    {
        get
        {
            var col = GetComponent<Collider2D>();
            LayerMask layerMask;
            if (Input.GetKey(KeyCode.S)) layerMask = LayerMask.GetMask("Environment");
            else layerMask = LayerMask.GetMask("Environment", "Platform");
            return Physics2D.Raycast(
                new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y),
                Vector2.down,
                -1f,
                layerMask);
        }
    }

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
    }

    public void RunAnimationStateMachine()
    {
        switch (currentAnimationState)
        {
            case AnimState.Idle:
                if (plr.CurrentState == BaseController.State.Ready)
                {
                    if (mc.MoveVector.x != 0) ChangeToRunState();
                    if (!mc.IsGrounded && !CloseToGround) ChangeToFallState();
                }
                else if (plr.CurrentState == BaseController.State.Action)
                {
                    if (plr.CurrentAction == PlayerController.Action.Rolling) ChangeToRollState();
                    else if (plr.CurrentAction == PlayerController.Action.Attacking) ChangeToAttackState();
                }
                break;

            case AnimState.Run:
                if (plr.CurrentState == BaseController.State.Ready)
                {
                    if (mc.MoveVector.x == 0) ChangeToIdleState();
                    if (!mc.IsGrounded && !CloseToGround) ChangeToFallState();
                }
                else if (plr.CurrentState == BaseController.State.Action)
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
                else if (plr.CurrentState == BaseController.State.Action)
                {
                    if (plr.CurrentAction == PlayerController.Action.Rolling) ChangeToRollState();
                }
                break;

            case AnimState.Rolling:
                if (plr.CurrentState != BaseController.State.Action || 
                    plr.CurrentAction != PlayerController.Action.Rolling)
                    ChangeToIdleState();
                break;

            case AnimState.Attacking:
                if (plr.CurrentState != BaseController.State.Action ||
                    plr.CurrentAction != PlayerController.Action.Attacking)
                    ChangeToIdleState();

                var newAttackState = am.AtkState;
                if (previousAttackState != newAttackState && newAttackState != PlayerAttackManager.AttackState.None)
                {
                    if (newAttackState == PlayerAttackManager.AttackState.Ranged)
                    {
                        sk_an.AnimationState.SetAnimation(0, "BowFirstShot", false);
                    }
                    else
                    {
                        switch (am.MeleeWeapon.Type)
                        {
                            case MeleeWeaponItem.WeaponType.LightSword:
                                SwitchOnLSword(newAttackState);
                                break;

                            case MeleeWeaponItem.WeaponType.Mace:
                                SwitchOnMace(newAttackState);
                                break;

                            case MeleeWeaponItem.WeaponType.Warhammer:
                                SwitchOnWarhammer(newAttackState);
                                break;

                            default:
                                throw new NotImplementedException("This attack animation type is not yet configured!");
                        }
                    }

                }
                previousAttackState = newAttackState;
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
    public void ChangeToRunState()
    {
        SetAnimationState(AnimState.Run);
        sk_an.AnimationState.SetAnimation(0, "OnehandRun", true);
    }
    public void ChangeToFallState()
    {
        SetAnimationState(AnimState.Fall);
        sk_an.AnimationState.SetAnimation(0, "Jump", false);
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

    private void SwitchOnLSword(PlayerAttackManager.AttackState newAttackState)
    {
        switch (newAttackState)
        {
            case PlayerAttackManager.AttackState.Ranged:
                break;

            case PlayerAttackManager.AttackState.N1:
                sk_an.AnimationState.SetAnimation(0, "LSwordString1", false);
                break;

            case PlayerAttackManager.AttackState.N2:
                sk_an.AnimationState.SetAnimation(0, "LSwordString2", false);
                break;

            case PlayerAttackManager.AttackState.N3:
                sk_an.AnimationState.SetAnimation(0, "LSwordString3", false);
                break;

            case PlayerAttackManager.AttackState.N4:
                sk_an.AnimationState.SetAnimation(0, "LSwordString4", false);
                break;

            case PlayerAttackManager.AttackState.N5:
                sk_an.AnimationState.SetAnimation(0, "LSwordString5", false);
                break;

            default:
                throw new NotImplementedException("This attack animation sub-state is not yet configured!");
        }
    }
    private void SwitchOnMace(PlayerAttackManager.AttackState newAttackState)
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

            default:
                throw new NotImplementedException("This attack animation sub-state is not yet configured!"); ;
        }
    }
    private void SwitchOnWarhammer(PlayerAttackManager.AttackState newAttackState)
    {
        switch (newAttackState)
        {
            case PlayerAttackManager.AttackState.N1:
                sk_an.AnimationState.SetAnimation(0, "AxeString1", false);
                break;

            case PlayerAttackManager.AttackState.N2:
                sk_an.AnimationState.SetAnimation(0, "AxeString2", false);
                break;
                

            default:
                throw new NotImplementedException("This attack animation sub-state is not yet configured!"); ;
        }
    }
    private void SetAnimationState(AnimState newAnimationState)
    {
        currentAnimationState = newAnimationState;
    }
}
