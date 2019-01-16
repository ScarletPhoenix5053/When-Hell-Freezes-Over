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
}
