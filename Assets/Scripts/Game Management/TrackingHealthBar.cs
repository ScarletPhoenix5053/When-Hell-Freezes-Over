using System;
using UnityEngine;

public class TrackingHealthBar : MonoBehaviour
{
    public Health Health;
    public Transform Meter;
    private Transform parent;

    private Vector3 meterPos;
    private Vector3 originalScale;
    private Vector3 offset;

    private void Awake()
    {
        if (Health == null) Debug.LogWarning(name + " has no health refrence! It will never update.");
        if (Meter == null) Debug.LogWarning(name + " has no meter object refrence! It will never update.");

        meterPos = Meter.localPosition;
        originalScale = Meter.localScale;
        offset = transform.localPosition;
        parent = transform.parent.GetComponentInChildren<BaseController>().transform;
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

        // Interpolate scale using percentage
        var newXScale = Mathf.Lerp(0, originalScale.x, percentage); 
        Meter.localScale = new Vector3(newXScale, originalScale.y, originalScale.z);

        // Interpolate position using percentage
        var newXPos = Mathf.Lerp(meterPos.x + originalScale.x / 2, meterPos.x, percentage);
        Meter.localPosition = new Vector3(newXPos, meterPos.y, meterPos.z);

        transform.localPosition = parent.localPosition + offset;
    }
}
