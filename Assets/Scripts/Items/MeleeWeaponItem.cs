using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Item/Weapon")]
public class MeleeWeaponItem : GenericItem
{
    public AttackData[] NormalAtkData;
    public AttackData SpecialAtkData;
    public WeaponType Type;


    public enum WeaponType
    {
        LongSword, GreatSword, Mace, Warhammer
    }

    public override GenericItem GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }
}