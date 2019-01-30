using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    public PlayerAttackManager pam;

    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    public List<GenericItem> equippedItems = new List<GenericItem>();

    public event Action<GenericItem> OnItemLeftClickedEvent;

    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    public bool AddItem(GenericItem item, out GenericItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].EquipmentType == item.equipmentType)
            {
                previousItem = (GenericItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                equippedItems.Add(item);

                if (item is MeleeWeaponItem)
                {
                    pam.MeleeWeapon = ((MeleeWeaponItem)item);
                }

                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(GenericItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                equippedItems.Remove(item); 
                return true;

            }
        }
        return false;
    }
}
