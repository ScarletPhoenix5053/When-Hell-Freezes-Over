using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType EquipmentType;

    protected override void Update()
    {
        if (Item is MeleeWeaponItem)
        {
            image.sprite = Item.Icon;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = EquipmentType.ToString() + "Slot";
    }
}
