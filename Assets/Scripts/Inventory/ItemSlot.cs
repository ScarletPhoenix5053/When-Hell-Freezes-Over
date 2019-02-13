using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<GenericItem> OnLeftClickEvent;
    public event Action<GenericItem> OnRightClickEvent;

    private Vector3 originalScale;
    public bool Grown;

    private GenericItem _item;
    public GenericItem Item
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
                //Not being enabled until I open the inventory before picking up the items.
                image.enabled = true;
                image.sprite = _item.Icon;

                if (_item is MeleeWeaponItem)
                {
                    image.sprite = _item.iconInventory;
                }
            }
        }
    }

    public Image image;
    [SerializeField] ItemTooltip tooltip;
    [SerializeField] Text amountText;

    private int _Amount;
    public int Amount
    {
        get { return _Amount; }
        set
        {
            _Amount = value;
            amountText.enabled = Item != null && _item.MaximumStacks > 1 && _Amount > 1;
            if(amountText.enabled)
            {
                amountText.text = _Amount.ToString();
            }
        }
    }

    protected virtual void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();

        if(tooltip == null)
        { 
            tooltip = FindObjectOfType<ItemTooltip>();
        }

        if(amountText == null)
        {
            amountText = GetComponentInChildren<Text>();
        }
    }

    void start()
    {
        originalScale = transform.localScale;
    }

    protected virtual void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if (Item != null && OnLeftClickEvent != null)
            {
                Debug.Log("Equipping");
                OnLeftClickEvent(Item);
            }
        }

        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
            {
                Debug.Log("Destroying.");
                OnRightClickEvent(Item);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(Item, transform.position);
        Grow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
        Shrink();
    }

  public void Grow()
  {
        if (!Grown)
        {
            transform.localScale += new Vector3(0.5F, 0.5f, originalScale.z);
            Grown = true;
        }
  }

  public void Shrink()
  {
        if (Grown)
        {
            transform.localScale -= new Vector3(0.5F, 0.5f, originalScale.z);
            Grown = false;
        }
  }

  


}
