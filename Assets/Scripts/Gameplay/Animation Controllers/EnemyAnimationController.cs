using UnityEngine;
using System.Collections;

public class EnemyAnimationController : AnimationController
{
    protected virtual void LateUpdate()
    {
        if (sk_an != null)
        {
            if (GameManager.Instance.HitStopActive) sk_an.timeScale = 0;
            else sk_an.timeScale = 1;
        }
    }

    public virtual void EffectOnMoveStart()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Attack", true);
    }
    public virtual void EffectOnMoveEnd()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Idle", true);
    }
    public virtual void EffectOnDeath()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Death", false);
    }
    public virtual void EffectOnHit()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "HitStun", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
}
