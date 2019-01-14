using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour , IItemContainer
{
    [SerializeField] List<GenericItem> startingItems;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    public event Action<GenericItem> OnItemLeftClickedEvent;

    private void Start()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }

        SetStartingItems();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
    }

    private void SetStartingItems()
    {
        int i = 0;
        for (; i < startingItems.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].item = Instantiate(startingItems[i]);
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].item = null;
        }
    }
    

    public bool AddItem(GenericItem item)
    {
        /*
        if (IsFull())
            return false;

        items.Add(item);
        RefreshUI();
        return true;
        */

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item == null)
            {
                itemSlots[i].item = item;
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(GenericItem item)
    {
        /*
        if (items.Remove(item))
        {
        RefreshUI();
        return true;
        }
        return false;
        */

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == item)
            {
                itemSlots[i].item = null;
                return true;
            }
        }

        return false;

    }

    public bool IsFull()
    {
        //return items.Count >= itemSlots.Length;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                return false;
            }
        }

        return true;
    }

    public GenericItem RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            GenericItem item = itemSlots[i].item;
            if(item != null && item.ID == itemID)
            {
                itemSlots[i].item = null;
                return item;
            }
        }
        return null;
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

    public int ItemCount(string itemID)
    {
        int number = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item.ID == itemID)
            {
                number++;
            }

        }
        return number;
    }

    
}

