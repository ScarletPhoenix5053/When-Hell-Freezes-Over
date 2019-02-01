using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] DestroyDialog dropCheck;
#pragma warning restore 0649

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
            FindObjectOfType<AudioManager>().Play("WeaponEquip"); //EQUIPPING WEAPON SOUNDS
        }

        if(item is ArmorItem)
        {
            Equip((ArmorItem)item);
            FindObjectOfType<AudioManager>().Play("ArmorEquip"); //EQUIPPING ARMOR SOUND
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
            FindObjectOfType<AudioManager>().Play("Unequip"); //UNEQUIPPING ITEMS SOUND
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
        FindObjectOfType<AudioManager>().Play("DestroyItem"); //DESTROYING ITEMS SOUND
        //Destroy(item);

    }
}
