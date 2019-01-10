﻿using UnityEngine;

[RequireComponent(typeof(MotionController))]
public class Health : MonoBehaviour
{
    public int Hp = 6;
    public int HpMax = 6;

    public bool Dead { get { return Hp <= 0; } }

    private MotionController mc;

    private void Awake()
    {
        mc = GetComponent<MotionController>();
    }

    public void Damage(AttackData data)
    {
        // Log warning and return if dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        // Adjust Hp
        if (data.Damage != 0) Hp -= data.Damage;
        if (Hp < 0) Hp = 0;

        if (mc != null)
        {
            mc.UpdateVelocity(new Vector3(data.KnockBack, data.KnockUp, 0));
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
}
