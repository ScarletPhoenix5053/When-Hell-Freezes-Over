using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Item/Weapon")]
public class MeleeWeaponItem : GenericItem
{
    [HideInInspector]
    public int NormalAttackChainLength;
    public AttackData[] NormalAtkData;
    public AttackData SpecialAtkData;
    public WeaponType Type;

    //public EquipmentType equipmentType;

    public enum WeaponType
    {
        LongSword, GreatSword, Mace, Warhammer
    }

    public void Refresh()
    {
        var newNormalAtkData = new AttackData[NormalAttackChainLength];

        if (NormalAtkData != null)
        {
            for (int i = 0; i < Mathf.Min(newNormalAtkData.Length, newNormalAtkData.Length); i++)
            {
                newNormalAtkData[i] = NormalAtkData[i];
            }
        }
        NormalAtkData = newNormalAtkData;
    }
}