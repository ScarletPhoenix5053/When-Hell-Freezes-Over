using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class AttackManager : MonoBehaviour, IHitboxResponder
{
    public bool Attacking = false;
    public Hitbox Hitbox;
    public AttackData[] Attacks;
    public AttackState AtkState = AttackState.None;

    protected const int numOfAttacks = 4;
    protected AttackType atkType = AttackType.Hit;
    protected IEnumerator activeCoroutine = null;

    public enum AttackType { Hit, Throw }
    public enum AttackState { None =-1, Light1, Light2, Light3, Launcher}
    

    private void Awake()
    {
        if (Attacks.Length < numOfAttacks) throw new NullReferenceException("Please make sure there are at least " + numOfAttacks + " attacks in Attacks array!");
    }
    protected void FixedUpdate()
    {
        Hitbox.UpdateHitbox();
    }

    // IHitboxResponder
    public void Hit(Collider hurtbox)
    {
        // Check hurtbox blockstate
        bool? success = false;
        if (atkType == AttackType.Hit)
        {
            switch (AtkState)
            {
                case AttackState.Light1:
                    success = hurtbox.GetComponent<Hurtbox>()?.
                        CheckHit(Attacks[0].BlockStun, Attacks[0].HitStun);
                    break;
                case AttackState.Light2:
                    success = hurtbox.GetComponent<Hurtbox>()?.
                        CheckHit(Attacks[1].BlockStun, Attacks[1].HitStun);
                    break;
                case AttackState.Light3:
                    success = hurtbox.GetComponent<Hurtbox>()?.
                        CheckHit(Attacks[2].BlockStun, Attacks[2].HitStun);
                    break;
                case AttackState.Launcher:
                    success = hurtbox.GetComponent<Hurtbox>()?.
                        CheckHit(Attacks[3].BlockStun, Attacks[3].HitStun);
                    break;
                default:
                    break;
            }
        }
        if (atkType == AttackType.Throw)
            success = hurtbox.GetComponent<Hurtbox>()?.CheckThrow();

        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        if ((bool)success)
        {
            switch (AtkState)
            {
                case AttackState.Light1:
                    hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[0]);
                    break;
                case AttackState.Light2:
                    hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[1]);
                    break;
                case AttackState.Light3:
                    hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[2]);
                    break;
                case AttackState.Launcher:
                    hurtbox.GetComponent<Hurtbox>().Health.Damage(Attacks[3]);
                    break;
                default:
                    break;
            }
            //hurtbox.GetComponent<Hurtbox>().Health.LogHp();
            Hitbox.SetInactive();
        } 
    }

    public bool LightAttack()
    {
        var success = false;
        if (AtkState == AttackState.None ||
            AtkState == AttackState.Light1 ||
            AtkState == AttackState.Light2)
        {
            atkType = AttackType.Hit;
            success = true;
            switch (AtkState)
            {
                case AttackState.None:
                    if (activeCoroutine != null) StopCoroutine(activeCoroutine);
                    activeCoroutine = DoAttack(0);
                    StartCoroutine(activeCoroutine);
                    SetAtkState(AttackState.Light1);
                    break;
                case AttackState.Light1:
                    if (activeCoroutine != null) StopCoroutine(activeCoroutine);
                    activeCoroutine = DoAttack(1);
                    StartCoroutine(activeCoroutine);
                    SetAtkState(AttackState.Light2);
                    break;
                case AttackState.Light2:
                    if (activeCoroutine != null) StopCoroutine(activeCoroutine);
                    activeCoroutine = DoAttack(2);
                    StartCoroutine(activeCoroutine);
                    SetAtkState(AttackState.Light3);
                    break;
                default:
                    throw new NullReferenceException("There is no light attack chain option from AttackState: " + AtkState);
            }
        }
        return success;
    }
    public bool LauncherAttack()
    {
        var success = false;
        if (AtkState == AttackState.None ||
            AtkState == AttackState.Light1 ||
            AtkState == AttackState.Light2 ||
            AtkState == AttackState.Light3)
        {
            atkType = AttackType.Hit;
            success = true;

            if (activeCoroutine != null) StopCoroutine(activeCoroutine);
            activeCoroutine = DoAttack(3);
            StartCoroutine(activeCoroutine);
            SetAtkState(AttackState.Launcher);
        }
        return success;
    }

    protected void SetAtkState(AttackState newState)
    {
        if (AtkState == newState) Debug.LogWarning("Tried to set state to " + newState + " when it already was " + newState + "!");
        else
        {
            Debug.Log("Changing state from " + AtkState + " to " + newState + ".");
            AtkState = newState;
        }
        
    }
    protected IEnumerator DoAttack(int attackIndex)
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
        SetAtkState(AttackState.None);
    }
}
