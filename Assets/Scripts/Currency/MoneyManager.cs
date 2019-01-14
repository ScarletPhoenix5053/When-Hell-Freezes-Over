using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText;
    public int currentGold;

    void Start()
    {
        currentGold = GlobalControl.Instance.currentGold;
        moneyText.text = " " + currentGold;
    }

    void Update()
    {

    }

    public void AddMoney(int goldToAdd)
    {
        currentGold += goldToAdd;
        moneyText.text = " " + currentGold;
        GlobalControl.Instance.currentGold = currentGold;
    }

    public void SubtractMoney(int goldToSubtract)
    {
        if (currentGold - goldToSubtract < 0)
        {
            Debug.Log("Ain't nothin gonna happen bruv.");
        }
        else
        {
            currentGold -= goldToSubtract;
            moneyText.text = " " + currentGold;
            GlobalControl.Instance.currentGold = currentGold;
        }
    }

    public void SaveMoney()
    {
        GlobalControl.Instance.currentGold = currentGold;
    }
}
