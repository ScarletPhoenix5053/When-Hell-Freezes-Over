using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<GenericItem> OnLeftClickEvent;

    [SerializeField]
    private GenericItem _item;
    public GenericItem item
    {
        get { return _item; }
        set
        { _item = value;

            if(_item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = _item.Icon;
                image.enabled = true;
            }
        }
    }

    [SerializeField] Image image;
    [SerializeField] ItemTooltip tooltip;

    protected virtual void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();

        if(tooltip == null)
        { 
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    public void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if(eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            //THE ERROR IS HERE.
            if (item != null && OnLeftClickEvent != null)
            {
                Debug.Log("Equipping");
                OnLeftClickEvent(item);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
