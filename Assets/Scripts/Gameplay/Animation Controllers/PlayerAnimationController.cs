using UnityEngine;
using System;

public class PlayerAnimationController : AnimationController
{
    /// <summary>
    /// Calls death animation and resets this component to a normal state.
    /// </summary>
    public void PlayDeath()
    {
        GetComponent<Renderer>().material.color = Color.white;
        /*
        an.SetBool("Dead", true);
        an.updateMode = AnimatorUpdateMode.Normal;
        StopAllCoroutines();*/
    }

    public void PlayHitStun()
    {
        throw new NotImplementedException();
        /*
        an.SetBool("InHitStun", true);
        an.updateMode = AnimatorUpdateMode.Normal;
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);*/
    }
    public void StopHitStun()
    {
        throw new NotImplementedException();
        /*
        an.SetBool("InHitStun", false);
        an.updateMode = AnimatorUpdateMode.Normal;
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);*/
    }
}
