using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour , IItemContainer
{
    [SerializeField] List<GenericItem> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    public event Action<GenericItem> OnItemLeftClickedEvent;

    private void Start()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].item = items[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].item = null;
        }
    }
    

    public bool AddItem(GenericItem item)
    {
        if (IsFull())
            return false;

        items.Add(item);
        RefreshUI();
        return true;

    }

    public bool RemoveItem(GenericItem item)
    {
        if (items.Remove(item))
        {
        RefreshUI();
        return true;
        }
        return false;
        
    }

    public bool IsFull()
    {
        return items.Count >= itemSlots.Length;
    }

    
    public bool ContainsItem(GenericItem item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item == item)
            {
                return true;
            }

        }
        return false;
    }

    public int ItemCount(GenericItem item)
    {
        int number = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == item)
            {
                number++;
            }

        }
        return number;
    }

    
}

