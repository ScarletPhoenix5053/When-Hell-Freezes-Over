﻿using System.Collections;
using System;
using UnityEngine;
using Sierra.Combat2D;

/// <summary>
/// Controller for ranged projeciles. Only allows travel along the X axis for now!
/// </summary>
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(MotionController))]
public class ProjectileController : MonoBehaviour, IHitboxResponder
{
    protected int sign = 1;
    protected Hitbox hb;
    protected MotionController mc;
    protected AttackData attackData;

    protected void Awake()
    {
        hb = GetComponent<Hitbox>();
        mc = GetComponent<MotionController>();
    }
    protected void OnEnable()
    {
        hb.SetActive();
    }
    protected void FixedUpdate()
    {
        hb.UpdateHitbox();

        mc.MoveVector = new Vector2(sign, 0);
        mc.UpdatePosition();
    }

    // IHitboxResponder
    public virtual void Hit(Collider2D hurtbox)
    {
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb != null)
        {
            if (hb.CheckHit(attackData.HitStun))
            {
                // set sign of attack
                attackData.Sign = sign;
                hurtbox.GetComponent<Hurtbox>().hp.Damage(attackData);
                //hurtbox.GetComponent<Hurtbox>().Health.LogHp();

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Sign controls which diection the projecile travels in. Processes value so there's no neet to convert it beforehand.
    /// </summary>
    /// <param name="newSign"></param>
    public void SetSign(int newSign)
    {
        sign = Math.Sign(newSign);
    }
    /// <summary>
    /// Assign <see cref="Hitbox.responder"/> for all attatched hitboxes.
    /// </summary>
    /// <param name="responder"></param>
    public void SetHitboxResponder(IHitboxResponder responder)
    {
        if (hb == null) hb = GetComponent<Hitbox>();
        hb.SetResponder(responder);
    }
    public void SetAttackData(AttackData newData)
    {
        attackData = newData;
    }
}
