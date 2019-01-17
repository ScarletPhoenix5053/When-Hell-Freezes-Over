using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    //Attach this to the gold items, maybe make a prefab. Can't do this til I have a character to test it with.
    public int value;
    public MoneyManager moneyManager;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            moneyManager.AddMoney(value);
            Destroy(gameObject);
        }
    }
}
