using UnityEngine;
using System.Collections;

public class DemonAnimationController : EnemyAnimationController
{
    public void EffectOnThrowAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "ThrowAttack", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
}
