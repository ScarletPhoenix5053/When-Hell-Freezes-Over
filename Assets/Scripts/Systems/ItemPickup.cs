using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GenericItem item;
    private Inventory inventoryPanel; //Put all the finished items in a group and assign this at once. It's annoying but it works. 
    private SpriteRenderer sprite;
    private PlayerInteract player;

    float originalY;
    float floatStrength = 0.5f;
    float floatSpeed;

    private void Start()
    {
        //Debug.LogWarning("Temporarily disabling item pickups ~ Sierra");
        //Destroy(gameObject);
        
        inventoryPanel = GameObject.Find("InventoryCanvas").transform.Find("Inventory/CharacterPanel/InventoryPanel").GetComponent<Inventory>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();

        sprite.sprite = item.Icon;
        floatSpeed = UnityEngine.Random.Range(2, 10);
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
        FindObjectOfType<AudioManager>().Play("ItemPickup");

        if (player.pickedUp == true)
            player.pickedUp = false;

        Destroy(gameObject);
    }

    public void Float()
    {
        if (player.pickedUp == false)
        {
            transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.time * floatSpeed) * floatStrength, transform.position.z);
            //transform.position = new Vector3(transform.position.x, originalY +
                //((float)Math.Sin(Time.time) * floatStrength), transform.position.z);
        }
    }
}
