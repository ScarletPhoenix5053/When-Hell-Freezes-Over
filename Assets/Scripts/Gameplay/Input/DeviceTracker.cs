using UnityEngine;
using System;

[RequireComponent(typeof(InputManager))]
public abstract class DeviceTracker : MonoBehaviour
{
    protected InputManager im;
    protected InputData data;
    /// <summary>
    /// True the when <see cref="data"/> has been modified.
    /// </summary>
    protected bool newData;

    protected void Awake()
    {
        im = GetComponent<InputManager>();
        data = new InputData(im.AxisCount, im.ButtonCount);
    }

    public abstract void Refresh();
}
[Serializable]
public struct AxisKeys
{
    public KeyCode Positive;
    public KeyCode Negative;
}