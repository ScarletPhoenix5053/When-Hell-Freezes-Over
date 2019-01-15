﻿using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour, IHitboxResponder
{
    public bool Attacking = false;
    public Hitbox Hitbox;
    public AttackData[] Attacks;
    public AttackStage AtkStage = AttackStage.Ready;
    public enum AttackStage { Ready, Startup, Active, Recovery }

    protected GameObject projectilePrefab;
    protected GameObject[] projectiles;

    protected int currentMeleeAttackIndex = 0;
    protected IEnumerator currentAttackRoutine = null;

    protected virtual void FixedUpdate()
    {
        Hitbox.UpdateHitbox();
    }

    // IHitboxResponder
    public virtual void Hit(Collider2D hurtbox)
    {
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb != null)
        {
            if (hb.CheckHit(Attacks[currentMeleeAttackIndex].HitStun))
            {
                // set sign of attack
                Attacks[currentMeleeAttackIndex].Sign = Math.Sign(transform.localScale.x);
                hurtbox.GetComponent<Hurtbox>().hp.Damage(Attacks[currentMeleeAttackIndex]);
                //hurtbox.GetComponent<Hurtbox>().Health.LogHp();

                Hitbox.SetInactive();
            }
        }
    }

    /// <summary>
    /// Generic Attack Method. <see cref="AttackData"/> used is determined by attackIndex.
    /// </summary>
    /// <param name="attackIndex"></param>
    public virtual void DoAttack(int attackIndex)
    {
        // Ensure index is in range
        if (attackIndex > Attacks.Length)
            throw new IndexOutOfRangeException(
                "Index " + attackIndex + " is out of range!" +
                name + "'s Attack's array size is " + Attacks.Length);

        // Start Coroutine
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
        currentAttackRoutine = IE_DoAttack(attackIndex);
        StartCoroutine(currentAttackRoutine);

        // Track which attack is ongoing
        currentMeleeAttackIndex = attackIndex;
    }
    /// <summary>
    /// Generic ranged attack method. Launches a projectile after a delay.
    /// </summary>
    /// <param name="attackIndex"></param>
    public virtual void DoRangedAttack(int attackIndex)
    {
        // Create Projectile
        projectiles = new GameObject[1];
        projectiles[0] = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);

        // Set projectile hitbox responder
        projectiles[0].GetComponent<ProjectileController>().SetHitboxResponder(this);

        // Start attack routine        
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
        currentAttackRoutine = IE_DoRangedAttack(attackIndex);
        StartCoroutine(currentAttackRoutine);
    }
    public virtual void StopAttack()
    {
        StopCoroutine(currentAttackRoutine);
        Hitbox.SetInactive();
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }
    protected virtual IEnumerator IE_DoAttack(int attackIndex)
    {
        // Startup
        Attacking = true;
        Hitbox.SetResponder(this);
        AtkStage = AttackStage.Startup;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Startup));

        // Active
        Hitbox.SetActive();
        AtkStage = AttackStage.Active;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Active));

        // Recovery
        Hitbox.SetInactive();
        AtkStage = AttackStage.Recovery;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Recovery));

        // End
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }
    protected virtual IEnumerator IE_DoRangedAttack(int attackIndex)
    {
        // Startup
        Attacking = true;
        AtkStage = AttackStage.Startup;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Startup));

        // Active
        projectiles[0].SetActive(true);
        AtkStage = AttackStage.Active;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Active));

        // Recovery
        AtkStage = AttackStage.Recovery;
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Recovery));

        // End
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }
}
