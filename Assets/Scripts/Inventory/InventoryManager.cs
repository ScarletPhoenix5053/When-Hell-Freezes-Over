using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] DestroyDialog dropCheck;

    private void Awake()
    {
        inventory.OnItemLeftClickedEvent += EquipFromInventory;
        inventory.OnItemRightClickedEvent += DestroyItem;
        equipmentPanel.OnItemLeftClickedEvent += UnequipFromEquipPanel;
    }

    private void EquipFromInventory(GenericItem item)
    {
        if(item is MeleeWeaponItem)
        {
            Equip((MeleeWeaponItem)item);
        }

        if(item is ArmorItem)
        {
            Equip((ArmorItem)item);
        }
    }

    private void UnequipFromEquipPanel(GenericItem item)
    {
        if (item is MeleeWeaponItem)
        {
            Unequip((MeleeWeaponItem)item);
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

    public void DestroyItem(GenericItem item)
    {
        if (item == null) return;

        dropCheck.Show();
        dropCheck.OnYesEvent += () => ActuallyDestroyItem(item);    
    }

    public void ActuallyDestroyItem(GenericItem item)
    {
        inventory.RemoveItem(item);
        //Destroy(item);

    }
}
