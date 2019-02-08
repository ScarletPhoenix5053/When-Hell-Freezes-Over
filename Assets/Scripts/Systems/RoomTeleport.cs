using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour
{
    public Transform Destination;
    public PlayerInteract pI;

    private void Start()
    {
        //Set the Player Interact here
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (pI.doorOpen == true)
            {
                //Fancy fade or whatever?
                other.transform.position = Destination.transform.position;
            }
            else if (!pI.doorOpen)
            {
                Debug.Log("Door is locked. Can't go here.");
            }
        }
    }
}
