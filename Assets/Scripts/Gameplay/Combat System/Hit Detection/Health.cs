using UnityEngine;
using System;
using System.Collections;
using Sierra.Combat2D;

[RequireComponent(typeof(MotionController))]
public class Health : MonoBehaviour
{
    public int Hp = 6;
    public int HpMax = 6;

    public Hurtbox[] Hurtboxes;

    public bool Dead { get { return Hp <= 0; } }

    private MotionController mc;
    private BaseController chr;

    private AttackData atkData;
    private IEnumerator currentKbRoutine;
    private IEnumerator currentHsRoutine;

    private void Awake()
    {
        chr = GetComponent<BaseController>();
        mc = GetComponent<MotionController>();
    }

    public void Damage(AttackData data)
    {
        atkData = data;
        // Log warning and return if ALREADY dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        // Adjust Hp
        if (data.Damage != 0) Hp -= data.Damage;

        // Check if died this frame
        if (Hp <= 0)
        {
            Hp = 0;
            chr.Die();
            StopAllCoroutines();
        }
        else
        {
            // Apply Knockback/Stun
            ApplyHitStun();
            ApplyKnockBack();
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

    private void ApplyHitStun()
    {
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = HitStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplyKnockBack()
    {
        var sign = Mathf.Sign(transform.localScale.x);
        mc?.DoImpulse(new Vector2(atkData.KnockBack * atkData.Sign, atkData.KnockUp));
    }
    private IEnumerator HitStunRoutine()
    {
        var chr = GetComponent<BaseController>();

        chr.SetState(BaseController.State.InHitstun);
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(atkData.HitStun));

        chr.SetState(BaseController.State.Ready);
    }
}