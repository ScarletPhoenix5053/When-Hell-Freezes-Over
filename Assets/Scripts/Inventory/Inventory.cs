using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ItemContainer
{
#pragma warning disable 0649
    [SerializeField] List<GenericItem> startingItems;
    [SerializeField] Transform itemsParent;
#pragma warning restore 0649

    public event Action<GenericItem> OnItemLeftClickedEvent;
    public event Action<GenericItem> OnItemRightClickedEvent;

    private void Start()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
            itemSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
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
            itemSlots[i].Item = startingItems[i].GetCopy();
            itemSlots[i].Amount = 1;
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
            itemSlots[i].Amount = 0;
        }
    }   
}

