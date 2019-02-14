using UnityEngine;
using System.Collections;

public class SlimePriestAnimationController : EnemyAnimationController
{
    public override void EffectOnHit()
    {
        sk_an.AnimationState.SetAnimation(0, "Hit", true);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
}
