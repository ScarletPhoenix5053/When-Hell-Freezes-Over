using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptAppear : MonoBehaviour
{
    public GameObject space;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            space.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            space.gameObject.SetActive(false);
        }
    }
}
