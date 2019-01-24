using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GenericItem item;
    private Inventory inventoryPanel; //Put all the finished items in a group and assign this at once. It's annoying but it works. 
    private SpriteRenderer sprite;
    private PlayerBrooke player;

    float originalY;
    float floatStrength = 0.5f;

    private void Start()
    {
        inventoryPanel = GameObject.Find("InventoryCanvas").transform.Find("Inventory/CharacterPanel/InventoryPanel").GetComponent<Inventory>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBrooke>();

        sprite.sprite = item.Icon;
        this.originalY = this.transform.position.y;

        Float();
    }

    private void Update()
    {
        Float();
    }

    public void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        inventoryPanel.AddItem(item);
        Destroy(gameObject);
    }

    public void Float()
    {
        if (player.pickedUp == false)
        {
            //RANDOMISE THE SPEED?
            transform.position = new Vector3(transform.position.x, originalY
                + ((float)Math.Sin(Time.time) * floatStrength), transform.position.z);
        }
    }
}
