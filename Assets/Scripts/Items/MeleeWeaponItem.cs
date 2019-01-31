using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Item/Weapon")]
public class MeleeWeaponItem : GenericItem
{
    public string WeaponSkinName = "default";
    [HideInInspector]
    public int NormalAttackChainLength;
    public AttackData[] NormalAtkData;
    public WeaponType Type;

    public enum WeaponType
    {
        LightSword, GreatSword, Mace, Warhammer
    }
    
    public override GenericItem GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
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