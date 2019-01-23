using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GenericItem item;
    public Inventory inventoryPanel; //Put all the finished items in a group and assign this at once. It's annoying but it works. 
    private SpriteRenderer sprite;

    private void Start()
    {
       sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = item.Icon;
    }

    public void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        inventoryPanel.AddItem(item);
        Destroy(gameObject);
    }
}
