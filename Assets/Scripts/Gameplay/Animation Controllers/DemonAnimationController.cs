using UnityEngine;
using System.Collections;

public class DemonAnimationController : EnemyAnimationController
{
    public void EffectOnThrowAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "ThrowAttack", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
    public void EffectOnMeleeAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "SwipeAttack", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
    public void EffectOnFall()
    {
        sk_an.AnimationState.SetAnimation(0, "Hit", false);
        sk_an.AnimationState.AddAnimation(0, "Fall", true, 0);
    }
    public void EffectOnImpact()
    {
        if ((GetComponent<BaseController>() && GetComponent<BaseController>().CurrentState == BaseController.State.SuperStun) ||
            (GetComponentInParent<BaseController>() && GetComponentInParent<BaseController>().CurrentState == BaseController.State.SuperStun))
        {
            sk_an.AnimationState.SetAnimation(0, "Grounded", true);
        }
    }
}
