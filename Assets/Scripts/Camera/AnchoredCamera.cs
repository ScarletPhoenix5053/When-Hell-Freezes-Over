using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchoredCamera : MonoBehaviour
{
    public string VisibilityZone = "VisiblitySprite";
    public List<SpriteRenderer> VisibilityZones = new List<SpriteRenderer>();
    public List<SpriteRenderer> ActiveVisibilityZones = new List<SpriteRenderer>();
    public List<SpriteRenderer> HiddenVisibilityZones = new List<SpriteRenderer>();

    /// <summary>
    /// Finds all visibility sprites in scene and loads them into <see cref="VisibilityZones"/>
    /// </summary>
    public void LoadVisibilitySprites()
    {
        VisibilityZones = new List<SpriteRenderer>();

        var visSprites = GameObject.FindGameObjectsWithTag(VisibilityZone);
        foreach (GameObject visSprite in visSprites)
        {
            SpriteRenderer sr;
            if (sr = visSprite.GetComponent<SpriteRenderer>()) VisibilityZones.Add(sr);
        }
    }
    public void ChangeActiveVisibilityZonesTo(SpriteRenderer[] newZones)
    {
        var indexesToCull = new List<int>();

        // Check currentley active zones
        for (int i = 0; i < ActiveVisibilityZones.Count; i++)
        {
            // If a newZone is already active, skip
            var alreadyActive = false;
            foreach (SpriteRenderer newZone in newZones)
            {
                if (newZone == ActiveVisibilityZones[i])
                {
                    alreadyActive = true;
                    break;
                }
            }
            // else mark for removal
            if (!alreadyActive)
            {
                indexesToCull.Add(i);
            }
        }
        // Remove marked zones (need to call fade?)
        foreach (int index in indexesToCull)
        {
            ActiveVisibilityZones.Remove(ActiveVisibilityZones[index]);
        }

        // Check newZones
        for (int i = 0; i < newZones.Length; i++)
        {
            // If a newZone is already active, skip
            var alreadyActive = false;
            foreach (SpriteRenderer activeZone in ActiveVisibilityZones)
            {
                if (activeZone == newZones[i])
                {
                    alreadyActive = true;
                    break;
                }
            }
            // else add it to active
            if (!alreadyActive)
            {
                ActiveVisibilityZones.Add(newZones[i]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadVisibilitySprites();
        }   
        if (Input.GetKeyDown(KeyCode.W))
        {
            var array = new SpriteRenderer[] { VisibilityZones[0] };
            ChangeActiveVisibilityZonesTo(array);
        }
    }
}
