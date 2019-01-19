using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    [SerializeField] GenericItem item; //Put the item you want the chest to have in this inspector slot
    [SerializeField] int amount = 1;
    [SerializeField] Inventory inventory; //Drag the players inventory
    [SerializeField] SpriteRenderer spriteRenderer; 
    [SerializeField] KeyCode itemPickupKeycode = KeyCode.E; //change as you like.

    private bool isInRange;
    private bool isEmpty;

    //Add a trigger collider to the itemchest

    private void OnValidate()
    {
        if(inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }

        spriteRenderer.sprite = item.Icon;
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if(isInRange && !isEmpty && Input.GetKeyDown(itemPickupKeycode))
        {
            GenericItem itemCopy = item.GetCopy();
            //Item tooltip isn't appearing
            if (inventory.AddItem(itemCopy))
            {
                amount--;
                if (amount == 0)
                {
                    isEmpty = true;
                    spriteRenderer.enabled = false;
                }
            }
            else
            {
                itemCopy.Destroy();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
            if (!isEmpty)
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = false;
            spriteRenderer.enabled = false;
        }
    }
}
