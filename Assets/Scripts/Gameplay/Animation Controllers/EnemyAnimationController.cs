using UnityEngine;
using System.Collections;

public class EnemyAnimationController : AnimationController
{
    /// <summary>
    /// Calls death animation and resets this component to a normal state.
    /// </summary>
    public void PlayDeath()
    {
        an.SetBool("Dead", true);
        an.updateMode = AnimatorUpdateMode.Normal;
        StopAllCoroutines();
    }

    public void PlayHitStun()
    {
        an.SetBool("InHitStun", true);
        an.updateMode = AnimatorUpdateMode.Normal;
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
    }
    public void StopHitStun()
    {
        //Debug.Log("Called");
        an.SetBool("InHitStun", false);
        an.updateMode = AnimatorUpdateMode.Normal;
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
    }
}
