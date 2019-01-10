using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttackManager))]
public class WeaponManager : MonoBehaviour
{
    public WeaponItem WeaponData;

    private PlayerAttackManager am;

    private void Awake()
    {
        am = GetComponent<PlayerAttackManager>();
    }
    /// <summary>
    /// Resets <see cref="PlayerAttackManager.Attacks"/> attatched to this gameobject. 
    /// Sets first entry to special attack data, and the remainder to the normal chain, in order.
    /// </summary>
    public void UpdateAttackManager()
    {
        var newAttacks = new AttackData[WeaponData.NormalAtkData.Length + 1];

        // Set frist entry to special attack.
        newAttacks[0] = WeaponData.SpecialAtkData;

        // Set remainder to normal chain.
        for (int i = 1; i < WeaponData.NormalAtkData.Length; i++)
        {
            newAttacks[i] = WeaponData.NormalAtkData[i - 1];
        }
    }
}