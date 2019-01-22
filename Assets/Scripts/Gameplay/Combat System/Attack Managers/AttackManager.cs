using Sierra;
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

    protected int currentAttackIndex = 0;
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
        var criticalHit = false;
        if (hb != null)
        {
            // if hit a button
            if (hb is ButtonHurtbox)
            {
                hb.CheckHit();
            }
            // else must have hit a character
            else if (hb.CheckHit(out criticalHit))
            {
                // set sign of attack
                Attacks[currentAttackIndex].Sign = Math.Sign(transform.localScale.x);
                hurtbox.GetComponent<Hurtbox>().hp.Damage(Attacks[currentAttackIndex]);
                GameManager.Instance.HitStopFor(Attacks[currentAttackIndex].HitStop);
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
        CheckAttackIndexIsInRange(attackIndex);
        currentAttackIndex = attackIndex;

        StartAttackRoutine();
    }
    /// <summary>
    /// Generic ranged attack method. Launches a projectile after a delay.
    /// </summary>
    /// <param name="attackIndex"></param>
    public virtual void DoRangedAttack(int attackIndex)
    {
        CheckAttackIndexIsInRange(attackIndex);
        currentAttackIndex = attackIndex;

        // Create Projectile
        projectiles = new GameObject[1];
        projectiles[0] = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);

        // Configure projectile
        var projControl = projectiles[0].GetComponent<ProjectileController>();
        projControl.SetAttackData(Attacks[0]);
        projControl.SetHitboxResponder(projControl);

        // Start attack routine
        StartRangedAttackRoutine();
    }
    /// <summary>
    /// Resets the currentley ative attack.
    /// </summary>
    public virtual void StopAttack()
    {
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
        Hitbox.SetInactive();
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }

    /// <summary>
    /// Creates an impulse that moves the characer basted on <see cref="AttackData.MotionOnAttack"/>
    /// </summary>
    protected void ApplyAttackMotion()
    {
        if (GetComponent<PlayerController>())
            GetComponent<CharacterMotionController>()?.
                DoImpulse(new Vector2(Attacks[currentAttackIndex].MotionOnAttack * GetComponent<PlayerController>().Sign, 0));
    }
    /// <summary>
    /// Throws an exception with a custom message if index is out of range
    /// </summary>
    /// <param name="attackIndex"></param>
    protected void CheckAttackIndexIsInRange(int attackIndex)
    {
        if (attackIndex > Attacks.Length)
            throw new IndexOutOfRangeException(
                "Index " + attackIndex + " is out of range!" +
                name + "'s Attack's array size is " + Attacks.Length);
    }

    protected void StartAttackRoutine()
    {
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
        currentAttackRoutine = IE_DoAttack();
        StartCoroutine(currentAttackRoutine);
    }
    protected void StartRangedAttackRoutine()
    {
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);
        currentAttackRoutine = IE_DoRangedAttack();
        StartCoroutine(currentAttackRoutine);
    }

    protected virtual IEnumerator IE_DoAttack()
    {
        var attackTimer = 0;

        // Startup
        Attacking = true;
        Hitbox.SetResponder(this);
        ApplyAttackMotion();
        AtkStage = AttackStage.Startup;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Startup, attackTimer);

        // Active
        Hitbox.SetActive();
        AtkStage = AttackStage.Active;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Active, attackTimer);

        // Recovery
        Hitbox.SetInactive();
        AtkStage = AttackStage.Recovery;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Recovery, attackTimer);

        // End
        GetComponent<BaseController>().SetState(BaseController.State.Ready);
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }
    protected virtual IEnumerator IE_DoRangedAttack()
    {
        var attackTimer = 0;

        // Startup
        Attacking = true;
        ApplyAttackMotion();
        AtkStage = AttackStage.Startup;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Startup, attackTimer);

        // Active
        projectiles[0].GetComponent<ProjectileController>().SetSign(Convert.ToInt32(transform.localScale.x));
        projectiles[0].transform.position = transform.position;
        projectiles[0].SetActive(true);
        AtkStage = AttackStage.Active;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Active, attackTimer);

        // Recovery
        AtkStage = AttackStage.Recovery;
        yield return Utility.FrameTimer(Attacks[currentAttackIndex].Recovery, attackTimer);

        // End
        GetComponent<BaseController>().SetState(BaseController.State.Ready);
        AtkStage = AttackStage.Ready;
        Attacking = false;
    }
}
