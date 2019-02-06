using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManagerTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            FindObjectOfType<FightManager>().GoToNextStage();
        }
    }
}
