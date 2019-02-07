using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] GenericItem item; //Put the item you want the chest to have in this inspector slot
    [SerializeField] int amount = 1;
    [SerializeField] Inventory inventory; //Drag the players inventory
    [SerializeField] SpriteRenderer spriteRenderer = null, chestRenderer = null;
#pragma warning restore 0649

    public Sprite closed;
    public Sprite open;

    private bool isInRange;
    private bool isEmpty;

    //Add a trigger collider to the itemchest

    private void OnValidate()
    {
        if(inventory == null)
        {
            inventory = GameObject.Find("InventoryCanvas").transform.Find("Inventory/CharacterPanel/InventoryPanel").GetComponent<Inventory>();
        }

        spriteRenderer.sprite = item.Icon;
        spriteRenderer.enabled = false;
        chestRenderer.sprite = closed;
    }

    private void Update()
    {
        if(isInRange && !isEmpty && InputManager.Interact())
        {
            GenericItem itemCopy = item.GetCopy();
            //Item tooltip isn't appearing
            if (inventory.AddItem(itemCopy))
            {
                FindObjectOfType<AudioManager>().Play("ItemPickup");
                amount--;
                if (amount == 0)
                {
                    isEmpty = true;
                    spriteRenderer.enabled = false;
                    chestRenderer.sprite = open;
                    FindObjectOfType<AudioManager>().Play("ChestOpen");
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
