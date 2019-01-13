﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MotionController))]
public class Health : MonoBehaviour
{
    public int Hp = 6;
    public int HpMax = 6;

    public bool Dead { get { return Hp <= 0; } }

    private MotionController mc;

    private AttackData atkData;
    private IEnumerator currentKbRoutine;

    private void Awake()
    {
        mc = GetComponent<MotionController>();
    }

    public void Remove(AttackData data)
    {
        atkData = data;
        // Log warning and return if dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        // Adjust Hp
        if (data.Damage != 0) Hp -= data.Damage;
        if (Hp < 0) Hp = 0;

        // Apply Knockback
        DoKnockBack();
    }

    public void LogHp()
    {
        Debug.Log(name + ": " + Hp + "/ " + HpMax);
    }
    public void LogDeath()
    {
        Debug.Log(name + " is dead");
    }

    private void DoKnockBack()
    {
        if (currentKbRoutine != null) StopCoroutine(currentKbRoutine);
        currentKbRoutine = KnockBackRoutine();
        StartCoroutine(currentKbRoutine);
    }
    private IEnumerator KnockBackRoutine()
    {
        Debug.Log("Knockback!");
        var plr = GetComponent<PlayerController>();
        var enm = GetComponent<BlobController>();
        var sign = Mathf.Sign(transform.localScale.x);

        mc?.EnableInputOverride();
        plr?.SetState(PlayerController.State.Hit);
        enm?.SetBehaviour(BlobController.Behaviour.Hit);
        mc?.DoImpulse(new Vector2(atkData.KnockBack, atkData.KnockUp));
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(atkData.HitStun));

        mc?.DisableInputOverride();
        plr?.SetState(PlayerController.State.Normal);
        enm?.SetBehaviour(BlobController.Behaviour.Idle);

    }
}