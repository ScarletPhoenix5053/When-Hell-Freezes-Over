using System;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public Collider2D solidCollider;
    public Collider2D belowTrigger;

    private Transform plr;  
    private Renderer rn;

    private Color originalColour;
    private Color halfColour;

    private bool playerBelow;

    private float lowestPoint;

    private void Awake()
    {
        rn = GetComponentInChildren<Renderer>();
        plr = FindObjectOfType<PlayerController>().gameObject.transform;

        if (solidCollider == null) throw new NullReferenceException(name + " needs a 2D collider!");
        if (plr == null) Debug.LogError("Could not find a player!");

        originalColour = rn.material.color;
        halfColour = new Color(rn.material.color.r/2, rn.material.color.g/2, rn.material.color.b/2, 0.5f);

        FindLowestPoint();
    }
    private void FixedUpdate()
    {
        if (plr != null)
        {
            if (playerBelow)
            {
                rn.material.color = halfColour;
                solidCollider.enabled = false;
                playerBelow = false;
            }
            else
            {
                rn.material.color = originalColour;
                solidCollider.enabled = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerBelow = true;
        }
    }

    private void FindLowestPoint()
    {
        lowestPoint = transform.position.y - solidCollider.bounds.extents.y;
    }
}
