using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    [SerializeField] GenericItem item; //Put the item you want the chest to have in this inspector slot
    [SerializeField] Inventory inventory; //Drag the players inventory
    [SerializeField] KeyCode itemPickupKeycode = KeyCode.E; //change as you like.

    private bool isInRange;
    private bool isEmpty;

    //Add a trigger collider to the itemchest

    private void Update()
    {
        if(isInRange && !isEmpty && Input.GetKeyDown(itemPickupKeycode))
        {
           inventory.AddItem(item.GetCopy());
           isEmpty = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
