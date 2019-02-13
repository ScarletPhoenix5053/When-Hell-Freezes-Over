using UnityEngine;
using System.Collections;

public class EnemyAnimationController : AnimationController
{
    public int ColorFlashFrames = 12;
    public Color ColourOnHit = new Color(1, 0, 0, 1);

    protected new Renderer renderer;
    protected IEnumerator currentFlashRoutine;

    protected override void Awake()
    {
        base.Awake();
        renderer = GetComponent<Renderer>();
    }    

    public virtual void EffectOnMoveStart()
    {
        if (sk_an == null) return;

        //sk_an.AnimationState.SetAnimation(0, "Attack", true);
    }
    public virtual void EffectOnMoveEnd()
    {
        sk_an.AnimationState.SetAnimation(0, "Idle", true); 
    }
    public virtual void EffectOnDeath()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Death", false);
    }
    public virtual void EffectOnHit()
    {
        if (renderer == null) return;
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "HitStun", false);
        //sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);

        //FlashMaterialColour(ColourOnHit);
    }
    public virtual void EffectOnCrit()
    {
        sk_an.AnimationState.SetAnimation(0, "SuperStunHit", false);
        sk_an.AnimationState.AddAnimation(0, "Dizzy", true, 0);
    }
    public virtual void EffectOnAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "Attack", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }

    protected void FlashMaterialColour(Color flashColour)
    {
        if (currentFlashRoutine != null) StopCoroutine(currentFlashRoutine);
        currentFlashRoutine = FlashMaterialColourRoutine(flashColour);
        StartCoroutine(currentFlashRoutine);
    }
    private IEnumerator FlashMaterialColourRoutine(Color flashColour)
    {
        var timer = 0;
        var originalMaterialColor = renderer.material.color;

        renderer.material.color = flashColour;
        while (timer < ColorFlashFrames) { yield return new WaitForFixedUpdate(); timer++; }

        renderer.material.color = originalMaterialColor;
    }
}
