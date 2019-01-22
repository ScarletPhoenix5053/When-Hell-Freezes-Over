using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GenericItem item;

    public void DoInteraction()
    {
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}
