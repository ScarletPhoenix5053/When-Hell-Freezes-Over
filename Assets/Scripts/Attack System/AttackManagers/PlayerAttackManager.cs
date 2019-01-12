using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class PlayerAttackManager : AttackManager, IHitboxResponder
{
    public MeleeWeaponItem MeleeWeapon;
    public RangedWeaponItem RangedWeapon;
    public AttackData SpecialAttackData { get { return Attacks[0]; } }
    public AttackData ArrowAttackData { get { return Attacks[1]; } }
    
    protected override void OnEnable()
    {
        if (MeleeWeapon == null) throw new NullReferenceException("Please assign an object to MeleeWeapon");
        if (RangedWeapon == null) throw new NullReferenceException("Please assign an object to RangedWeapon");
        else AssignWeaponAttackData();
    }

    public bool NormalAttack()
    {
        var success = false;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = DoAttack(0);
        StartCoroutine(activeCoroutine);

        return success;
    }
    /// <summary>
    /// Re-populates <see cref="Attacks"/> array based on
    /// <see cref="MeleeWeapon"/> and <see cref="RangedWeapon"/>
    /// </summary>
    public void AssignWeaponAttackData()
    {
        AttackData[] newAttacks = null;

        // Melee
        if (MeleeWeapon != null)
        {
            newAttacks = new AttackData[MeleeWeapon.NormalAtkData.Length + 2];
            newAttacks[1] = MeleeWeapon.SpecialAtkData;
            for (int i = 0; i < MeleeWeapon.NormalAtkData.Length; i++)
            {
                newAttacks[i + 2] = MeleeWeapon.NormalAtkData[i];
            }
        }

        // Ranged
        if (RangedWeapon != null)
        {
            if (newAttacks == null) newAttacks = new AttackData[1];
            newAttacks[0] = RangedWeapon.ProjectileAttackData;
        }

        Attacks = newAttacks;
    }
}
