using UnityEngine;
using System;

public class PlayerAnimationController : AnimationController
{
    private Renderer rn;
    private Color defaultColour;

    protected override void Awake()
    {
        base.Awake();
        rn = GetComponent<Renderer>();
        defaultColour = rn.material.color;
    }

    public void PlayDodgeRoll()
    {
        rn.material.color = Color.yellow;
        an.SetBool("Rolling", true);
    }
    /// <summary>
    /// Temporary method. Currentley resets this <see cref="Renderer.material.color"/>
    /// </summary>
    public void PlayIdle()
    {
        rn.material.color = defaultColour;
        an.SetBool("Rolling", false);
    }
    /// <summary>
    /// Calls death animation and resets this component to a normal state.
    /// </summary>
    public void PlayDeath()
    {
        rn.material.color = Color.white;
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
