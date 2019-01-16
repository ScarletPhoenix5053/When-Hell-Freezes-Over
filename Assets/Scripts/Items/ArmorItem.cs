using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Armor Data", menuName = "Item/Armor")]
public class ArmorItem : GenericItem
{
    public int DefenseBonus;

    public override GenericItem GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }
}
