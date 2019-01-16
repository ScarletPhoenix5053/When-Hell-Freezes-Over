using System;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Transform plr;
    private Collider2D col;
    private Renderer rn;

    private Color originalColour;
    private Color halfColour;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rn = GetComponent<Renderer>();
        plr = FindObjectOfType<PlayerController>().gameObject.transform;

        if (col == null) throw new NullReferenceException(name + " needs a 2D collider!");
        if (plr == null) Debug.LogError("Could not find a player!");

        originalColour = rn.material.color;
        halfColour = new Color(rn.material.color.r/2, rn.material.color.g/2, rn.material.color.b/2, 0.5f);
    }
    private void FixedUpdate()
    {
        if (plr != null)
        {
            if (plr.position.y-1 < transform.position.y)
            {

                rn.material.color = halfColour;
                col.enabled = false;
            }
            else
            {
                rn.material.color = originalColour;
                col.enabled = true;
            }
        }
    }
}
