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

    protected const int numOfAttacks = 1;
    protected IEnumerator activeCoroutine = null;
    
    

    protected virtual void Awake()
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
    public virtual void Hit(Collider hurtbox)
    {
        // Check hurtbox blockstate
        bool? success = false;
        success = hurtbox.GetComponent<Hurtbox>()?.
            CheckHit(Attacks[0].BlockStun, Attacks[0].HitStun);


        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.
        if ((bool)success)
        {
            hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[0]);
            //hurtbox.GetComponent<Hurtbox>().Health.LogHp();

            Hitbox.SetInactive();
        } 
    }
    /*
    protected void SetAtkState(AttackState newState)
    {
        if (AtkState == newState) Debug.LogWarning("Tried to set state to " + newState + " when it already was " + newState + "!");
        else
        {
            Debug.Log("Changing state from " + AtkState + " to " + newState + ".");
            AtkState = newState;
        }
        
    }*/
    protected virtual IEnumerator DoAttack(int attackIndex)
    {
        // Startup
        Attacking = true;
        Hitbox.SetResponder(this);
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Startup));

        // Active
        Hitbox.SetActive();
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Active));

        // Recovery
        Hitbox.SetInactive();
        yield return new WaitForSeconds(Utility.FramesToSeconds(Attacks[attackIndex].Recovery));

        // End
        Attacking = false;
    }
}
