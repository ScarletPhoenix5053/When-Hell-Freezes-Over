using UnityEngine;

public class KeyboardTracker : DeviceTracker
{
    public AxisKeys[] AxisKeys;
    public KeyCode[] ButtonKeys;

    protected void Reset()
    {
        im = GetComponent<InputManager>();
        ButtonKeys = new KeyCode[im.ButtonCount];
        AxisKeys = new AxisKeys[im.AxisCount];
    }
    protected void Update()
    {
        // check for inputs
        CheckNewAxisInput();
        CheckNewButtonInput();
        if (newData) PassNewData();
    }

    public override void Refresh()
    {
        im = GetComponent<InputManager>();

        var newButtons = new KeyCode[im.ButtonCount];
        var newAxis = new AxisKeys[im.AxisCount];

        if (ButtonKeys != null)
        {
            for (int i = 0; i < Mathf.Min(newButtons.Length, ButtonKeys.Length); i++)
            {
                newButtons[i] = ButtonKeys[i];
            }
        }
        ButtonKeys = newButtons;
        if (AxisKeys != null)
        {
            for (int i = 0; i < Mathf.Min(newAxis.Length, AxisKeys.Length); i++)
            {
                newAxis[i] = AxisKeys[i];
            }
        }
        AxisKeys = newAxis;
    }

    protected void CheckNewButtonInput()
    {
        for (int i = 0; i < ButtonKeys.Length; i++)
        {
            if (Input.GetKeyDown(ButtonKeys[i]))
            {
                data.buttons[i] = true;
                newData = true;
            }
        }
    }
    protected void CheckNewAxisInput()
    {
        for (int i = 0; i < AxisKeys.Length; i++)
        {
            var val = 0f;
            if (Input.GetKey(AxisKeys[i].Positive))
            {
                val += 1;
                newData = true;
            }
            if (Input.GetKey(AxisKeys[i].Negative))
            {
                val -= 1;
                newData = true;
            }
            data.axes[i] = val;
        }
    }
    protected void PassNewData()
    {
        im.PassInput(data);
        data.Reset();
        newData = false;
    }
}