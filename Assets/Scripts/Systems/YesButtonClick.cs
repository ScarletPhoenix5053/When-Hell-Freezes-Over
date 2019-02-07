using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesButtonClick : MonoBehaviour
{
    Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
    }

    void Update()
    {
        if(InputManager.Jump())
        {
            thisButton.onClick.Invoke();
        }
    }
}
