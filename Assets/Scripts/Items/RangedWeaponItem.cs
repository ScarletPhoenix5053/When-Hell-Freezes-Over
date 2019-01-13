using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon Data", menuName = "Item/Ranged Weapon")]
public class RangedWeaponItem : GenericItem
{
    public ProjectileAttackData ProjectileAttackData;
    public WeaponType Type;

    public enum WeaponType
    {
        Single, Scatter
    }
}