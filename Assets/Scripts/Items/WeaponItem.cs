﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Item/Weapon")]
public class WeaponItem : GenericItem
{
    public AttackData[] NormalAtkData;
    public AttackData SpecialAtkData;
    public WeaponType Type;

    public enum WeaponType
    {
        LongSword, GreatSword, Mace, Warhammer
    }
}