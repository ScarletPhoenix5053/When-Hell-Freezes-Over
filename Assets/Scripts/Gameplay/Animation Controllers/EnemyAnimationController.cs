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
}
