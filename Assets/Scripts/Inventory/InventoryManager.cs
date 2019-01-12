using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    private void Awake()
    {
        inventory.OnItemLeftClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemLeftClickedEvent += UnequipFromEquipPanel;
    }

    private void EquipFromInventory(GenericItem item)
    {
        Debug.Log("oof");
        if(item is WeaponItem)
        {
            Equip((WeaponItem)item);
        }

        if(item is ArmorItem)
        {
            Equip((ArmorItem)item);
        }
    }

    private void UnequipFromEquipPanel(GenericItem item)
    {
        if (item is WeaponItem)
        {
            Unequip((WeaponItem)item);
        }

        if (item is ArmorItem)
        {
            Unequip((ArmorItem)item);
        }
    }

    public void Equip(GenericItem item)
    {
        if(inventory.RemoveItem(item))
        {
            GenericItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    Debug.Log("here?");
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(GenericItem item)
    {
        if(!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }
    }
}
