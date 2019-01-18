using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestroyDialog : MonoBehaviour
{
    public event Action OnYesEvent;
    public event Action OnNoEvent;

    public void OnYesClick()
    {
        if (OnYesEvent != null)
            OnYesEvent();

        Hide();
    }

    public void OnNoClick()
    {
        if (OnNoEvent != null)
            OnNoEvent();

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        OnYesEvent = null;
        OnNoEvent = null;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
