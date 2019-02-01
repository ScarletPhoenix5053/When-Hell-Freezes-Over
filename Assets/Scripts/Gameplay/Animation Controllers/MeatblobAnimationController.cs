using UnityEngine;
using System.Collections;

public class MeatblobAnimationController : EnemyAnimationController
{
    public override void EffectOnAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "Launch", false);
        sk_an.AnimationState.AddAnimation(0, "Mid-Air", true, 0);
        Debug.Log("Atk");
    }
    public virtual void EffectOnLand()
    {
        sk_an.AnimationState.SetAnimation(0, "Landing", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
        Debug.Log("Land");
    }
}