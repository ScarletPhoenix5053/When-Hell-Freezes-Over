using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlayerBrooke playerLadder;

    void Start()
    {
        playerLadder = FindObjectOfType<PlayerBrooke>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerLadder.OnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerLadder.OnLadder = false;
        }
    }
}
