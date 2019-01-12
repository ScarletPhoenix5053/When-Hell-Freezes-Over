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
    public AttackState AtkState = AttackState.None;
    public enum AttackState
    {
        None, N1, N2, N3, N4, N5, Special, Ranged, RangedSpecial
    }
    
    protected override void OnEnable()
    {
        if (MeleeWeapon == null) throw new NullReferenceException("Please assign an object to MeleeWeapon");
        if (RangedWeapon == null) throw new NullReferenceException("Please assign an object to RangedWeapon");
        else AssignWeaponAttackData();
    }

    protected override IEnumerator IE_DoAttack(int attackIndex)
    {
        yield return base.IE_DoAttack(attackIndex);
        SetAtkState(AttackState.None);
    }
    public void NormalAttack()
    {
        // DoAttack Takes an int index to use in Attacks Array.
        // With how i've configured it, Attacks[0] and [1] are used for ranged and special respectivley
        // Attacks[2]+ are for the normal attack chain.
        // For clarity i will use this int array to refer to the correct values
        // count from [1] not [0]
        int[] normalAttack = new int[] { -1, 2, 3, 4, 5, 6 };


        // Check if can chain   
        switch (AtkState)
        {
            case AttackState.None: // Allow attack
                DoAttack(normalAttack[1]);
                SetAtkState(AttackState.N1);
                break;

            case AttackState.N1:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 2) DoAttack(normalAttack[2]); else goto default;
                SetAtkState(AttackState.N2);
                break;

            case AttackState.N2:  // Allow attack if weapon allows chaining to the next && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 3) DoAttack(normalAttack[3]); else goto default;
                SetAtkState(AttackState.N3);
                break;

            case AttackState.N3:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 4) DoAttack(normalAttack[4]); else goto default;
                SetAtkState(AttackState.N4);
                break;

            case AttackState.N4:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 5) DoAttack(normalAttack[5]); else goto default;
                SetAtkState(AttackState.N5);
                break;

            default: // Do not allow attack if any other state
                Debug.Log(MeleeWeapon.Name + " does not allow chaining to this extent");
                break;
        }
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
    
    protected void SetAtkState(AttackState newState)
    {
        if (AtkState == newState) Debug.LogWarning("Tried to set state to " + newState + " when it already was " + newState + "!");
        else
        {
            Debug.Log("Changing state from " + AtkState + " to " + newState + ".");
            AtkState = newState;
        }

    }
}
