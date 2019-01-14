using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Range(0,10)]
    public int AxisCount;
    [Range(0,20)]
    public int ButtonCount;

    public PlayerController controller;

    public void PassInput(InputData data)
    {
        //Debug.Log(data.axes[0] + "," + data.axes[1]);
        controller.ReadInput(data);
    }

    public void RefreshTracker()
    {
        var devTracker = GetComponent<DeviceTracker>();
        if (devTracker != null)
        {
            devTracker.Refresh();
        }
        else
        {
            Debug.LogWarning("no device tracker is attatched to gameobject " + name + "! Please assign one.");
        }
    }
}
public struct InputData
{
    public float[] axes;
    public bool[] buttons;

    public InputData(int axisCount, int buttonCount)
    {
        axes = new float[axisCount];
        buttons = new bool[buttonCount];
    }

    public void Reset()
    {
        for (int i = 0; i < axes.Length; i++)
        {
            axes[i] = 0f;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = false;
        }
        
    }
}