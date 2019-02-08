using Spine.Unity;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class PlayerAttackManager : AttackManager, IHitboxResponder
{
    public MeleeWeaponItem MeleeWeapon;
    public RangedWeaponItem RangedWeapon;
    public int ArrowCapacity = 3;
    [ReadOnly]
    public int Arrows = 3;

    public AttackData SpecialAttackData { get { return Attacks[0]; } }
    public AttackData ArrowAttackData { get { return Attacks[1]; } }
    public AttackState AtkState = AttackState.None;
    public enum AttackState
    {
        None, N1, N2, N3, N4, N5, Special, Ranged, RangedSpecial
    }

    protected MeleeWeaponItem meleeItem;
    protected RangedWeaponItem rangedItem;


    protected virtual void OnEnable()
    {
        if (MeleeWeapon == null) throw new NullReferenceException("Please assign an object to MeleeWeapon");
        if (RangedWeapon == null) throw new NullReferenceException("Please assign an object to RangedWeapon");
        else AssignWeaponAttackData();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // If either weapon item changes
        if (meleeItem != MeleeWeapon ||
            rangedItem != RangedWeapon)
        {
            meleeItem = MeleeWeapon;
            rangedItem = RangedWeapon;

            // update attackData array
            AssignWeaponAttackData();
        }
    }

    protected override IEnumerator IE_DoAttack()
    {
        yield return base.IE_DoAttack();
        SetAtkState(AttackState.None);
    }
    protected override IEnumerator IE_DoRangedAttack()
    {
        yield return base.IE_DoRangedAttack();
        SetAtkState(AttackState.None);
    }
    /// <summary>
    /// Performs normal attack actions. Automatically handles chaining.
    /// </summary>
    public void NormalAttack()
    {
        // DoAttack Takes an int index to use in Attacks Array.
        // With how i've configured it, Attacks[0] and [1] are used for ranged and special respectivley
        // Attacks[2]+ are for the normal attack chain.
        // For clarity i will use this int array to refer to the correct values
        // count from [1] not [0]


        // Check if can chain   
        switch (AtkState)
        {
            case AttackState.None: // Allow attack
                DoAttack(1);
                SetAtkState(AttackState.N1);
                break;

            case AttackState.N1:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 2) DoAttack(2); else goto default;
                SetAtkState(AttackState.N2);
                break;

            case AttackState.N2:  // Allow attack if weapon allows chaining to the next && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 3) DoAttack(3); else goto default;
                SetAtkState(AttackState.N3);
                break;

            case AttackState.N3:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 4) DoAttack(4); else goto default;
                SetAtkState(AttackState.N4);
                break;

            case AttackState.N4:  // Allow attack if weapon allows chaining to the next attack && in recovery
                if (AtkStage != AttackStage.Recovery) return;
                if (MeleeWeapon.NormalAtkData.Length >= 5) DoAttack(5); else goto default;
                SetAtkState(AttackState.N5);
                break;

            default: // Do not allow attack if any other state
                Debug.Log(MeleeWeapon.Name + " does not allow chaining to this extent");
                break;
        }
    }
    /// <summary>
    /// Perfroms ranged attack actions.
    /// </summary>
    public void RangedAttack()
    {
        if (AtkStage == AttackStage.Ready)
        {
            Arrows -= 1;
            SetAtkState(AttackState.Ranged);
            DoRangedAttack(0);
        }
    }
    /// <summary>
    /// Re-populates <see cref="Attacks"/> array based on
    /// <see cref="MeleeWeapon"/> and <see cref="RangedWeapon"/>.
    /// Also sets <see cref="AttackManager.projectilePrefab"/> when a ranged weapon is present.
    /// </summary>
    public void AssignWeaponAttackData()
    {
        AttackData[] newAttacks = null;

        // Melee
        if (MeleeWeapon != null)
        {
            newAttacks = new AttackData[MeleeWeapon.NormalAtkData.Length + 1];
            for (int i = 0; i < MeleeWeapon.NormalAtkData.Length; i++)
            {
                newAttacks[i + 1] = MeleeWeapon.NormalAtkData[i];
            }
        }

        // Ranged
        if (RangedWeapon != null)
        {
            if (newAttacks == null) newAttacks = new AttackData[1];
            newAttacks[0] = RangedWeapon.ProjectileAttackData;
            projectilePrefab = RangedWeapon.ProjectilePrefab;
        }

        Attacks = newAttacks;

        am.SetSkin(MeleeWeapon.WeaponSkinName);
    }
    
    protected void SetAtkState(AttackState newState)
    {
        if (AtkState == newState) Debug.LogWarning("Tried to set state to " + newState + " when it already was " + newState + "!");
        else
        {
            //Debug.Log("Changing state from " + AtkState + " to " + newState + ".");
            AtkState = newState;
        }

    }
}
