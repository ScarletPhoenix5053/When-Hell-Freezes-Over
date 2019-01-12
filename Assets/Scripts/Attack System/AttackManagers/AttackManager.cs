using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour, IHitboxResponder
{
    public bool Attacking = false;
    public Hitbox Hitbox;
    public AttackData[] Attacks;
    public AttackStage AtkStage = AttackStage.Ready;
    public enum AttackStage { Ready, Startup, Active, Recovery }

    protected const int numOfAttacks = 1;
    protected IEnumerator activeCoroutine = null;

    protected virtual void OnEnable()
    {
        if (Attacks.Length < numOfAttacks)
            throw new NullReferenceException(
                "Please make sure there are at least " 
                + numOfAttacks 
                + " attacks in Attacks array!");
    }
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
            if (hb.CheckHit(Attacks[0].HitStun))
            {
                hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[0]);
                //hurtbox.GetComponent<Hurtbox>().Health.LogHp();

                Hitbox.SetInactive();
            }
        }
    }

    protected virtual void DoAttack(int attackIndex)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = IE_DoAttack(0);
        StartCoroutine(activeCoroutine);
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
}
