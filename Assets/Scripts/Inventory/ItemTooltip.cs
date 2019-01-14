﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text itemNameText;
    [SerializeField] Text slotText;
    [SerializeField] Text descText;

    public GameObject tooltip;

    public void ShowTooltip(GenericItem item, Vector3 position)
    {
        if (item != null)
        {
            itemNameText.text = item.Name;
            descText.text = item.Desc;
            slotText.text = item.equipmentType.ToString();

            gameObject.SetActive(true);

            tooltip.transform.position = position;
        }
        
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
