using UnityEngine;
using System;
using System.Collections;
using Sierra;
using Sierra.Combat2D;

[RequireComponent(typeof(CharacterMotionController))]
public class Health : MonoBehaviour
{
    public int HpMax = 6;
    public int Hp = 6;
    public float SuperStunMultiplier = 3f;

    public bool AffectedByKnockback = true;
    public bool AffectedByKnockbackOnCrit = true;

    public Hurtbox Hurtbox;

    public bool Dead { get { return Hp <= 0; } }

    private CharacterMotionController mc;
    private BaseController chr;

    private AttackData atkData;
    private IEnumerator currentHsRoutine;

    private void Awake()
    {
        chr = GetComponent<BaseController>();
        mc = GetComponent<CharacterMotionController>();
        Hurtbox = GetComponent<Hurtbox>();
    }

    public void Damage(AttackData data, bool critical = false)
    {
        Debug.Log(name + "was damaged");

        // Log warning and return if ALREADY dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        atkData = data;
        AdjustHP();

        // Check if died this frame
        if (Dead) Die();
        else
        {
            // Check for critical hit
            if (critical)
            {
                ApplySuperStun();
                ApplyKnockBack();
            }
            else
            {
                ApplyHitStun();
                ApplyKnockBack();
            }
        }
    }

    public void LogHp()
    {
        Debug.Log(name + ": " + Hp + "/ " + HpMax);
    }
    public void LogDeath()
    {
        Debug.Log(name + " is dead");
    }

    private void AdjustHP()
    {

        if (atkData.Damage != 0)
        {
            if (chr?.CurrentState == BaseController.State.SuperStun)
            {
                Hp -= Convert.ToInt32(atkData.Damage * SuperStunMultiplier);
            }
            else
            {
                Hp -= atkData.Damage;
            }
        }
        LogHp();
    }
    private void ApplyHitStun()
    {
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = HitStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplySuperStun()
    {
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = SuperStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplyKnockBack()
    {
        if (!AffectedByKnockback) return;
        if (Hurtbox.CurrentState == Hurtbox.State.Critical && !AffectedByKnockbackOnCrit) return;

        var sign = Mathf.Sign(transform.localScale.x);
        mc?.DoImpulse(new Vector2(atkData.KnockBack * atkData.Sign, atkData.KnockUp));
    }
    private void Die()
    {
        Hp = 0;
        chr?.Die();
        StopAllCoroutines();
    }

    private IEnumerator HitStunRoutine()
    {
        chr?.SetState(BaseController.State.HitStun);

        // Start timer
        yield return Utility.FrameTimer(atkData.HitStun, 0);

        // End timer
        chr?.SetState(BaseController.State.Ready);
    }
    private IEnumerator SuperStunRoutine()
    {
        Debug.Log("Applying Superstun for 3.5 seconds");
        chr?.SetState(BaseController.State.SuperStun);

        // Start timer
        yield return Utility.FrameTimer(210, 0);

        // End timer
        chr?.SetState(BaseController.State.Ready);
    }
}