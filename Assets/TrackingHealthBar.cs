using System;
using UnityEngine;

public class TrackingHealthBar : MonoBehaviour
{
    public Health Health;
    public Transform Meter;

    private void Awake()
    {
        if (Health == null) Debug.LogWarning(name + " has no health refrence! It will never update.");
        if (Meter == null) Debug.LogWarning(name + " has no meter object refrence! It will never update.");
    }

    private void FixedUpdate()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (Health == null || Meter == null) return;

        // Get percentage of bar
        var percentage = Convert.ToSingle(Health.Hp) / Convert.ToSingle(Health.HpMax);

        // Update 
        var initialScale = 2f;
        var initialPos = 0f;
        var endPos = initialPos - initialScale / 2;

        var interpolatedPos = Mathf.Lerp(initialPos, endPos, percentage);
        var interpolatedScale = Mathf.Lerp(0, initialScale, percentage);

        Meter.position = 
            new Vector3(
                Meter.transform.localPosition.x + interpolatedPos,
                transform.position.y + Meter.transform.localPosition.y,
                transform.position.z + Meter.transform.localPosition.z
                );
        Meter.localScale = new Vector3(interpolatedScale, Meter.localScale.y, Meter.localScale.z);
    }
}
