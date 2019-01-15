using System.Collections;
using Sierra;
using UnityEngine;

public class BlobAnimationController : EnemyAnimationController
{
    public void PlayAttack()
    {
        an.SetTrigger("Leap");
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
        animPhysCoroutine = AnimatePhysicsFor(100);
        StartCoroutine(animPhysCoroutine);
    }
    public void PlayHitStun()
    {
        an.SetBool("InHitStun", true);
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
        an.updateMode = AnimatorUpdateMode.Normal;
    }
    public void StopHitStun()
    {        
        an.SetBool("InHitStun", false);
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
        an.updateMode = AnimatorUpdateMode.Normal;
    }
}
