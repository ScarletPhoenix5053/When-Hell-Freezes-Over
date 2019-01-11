using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class PlayerAttackManager : AttackManager, IHitboxResponder
{
    public WeaponItem CurrentWeapon;
    public AttackData SpecialAttackData { get { return Attacks[0]; } }
    public AttackData ArrowAttackData { get { return Attacks[1]; } }
    
    protected override void OnEnable()
    {
        if (CurrentWeapon == null) throw new NullReferenceException("Please assign an object to CurrentWeapon");
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

    private void AssignWeaponAttackData()
    {
        var newAttacks = new AttackData[CurrentWeapon.NormalAtkData.Length + 2];

        newAttacks[0] = CurrentWeapon.SpecialAtkData;
        for (int i = 0; i < CurrentWeapon.NormalAtkData.Length; i++)
        {
            newAttacks[i + 2] = CurrentWeapon.NormalAtkData[i];
        }

        Attacks = newAttacks;
    }
}
