using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventRaiser : MonoBehaviour
{
    public UnityEvent Event;

    public void RaiseEvent()
    {
        if (Event != null) Event.Invoke();
    }
}
