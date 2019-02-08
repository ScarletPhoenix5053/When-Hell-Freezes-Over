using System;
using System.Collections;
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

    private const int PlayerLayer = 9;
    private const int PlatformLayer = 13;
    private float lowestPoint;
    private IEnumerator updateRoutine;

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
    private void OnEnable()
    {
        updateRoutine = UpdatePlatformState();
        StartCoroutine(updateRoutine);
    }
    private void OnDisable()
    {
        StopCoroutine(updateRoutine);
    }
    private void Update()
    {
        if (plr != null)
        {
            if (InputManager.HoldingDown())
            {
                rn.material.color = halfColour;
                transform.GetChild(0).gameObject.layer = PlayerLayer;
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
    private IEnumerator UpdatePlatformState()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (plr != null)
            {
                if (playerBelow || InputManager.HoldingDown())
                {
                    rn.material.color = halfColour;
                    transform.GetChild(0).gameObject.layer = PlayerLayer;
                    playerBelow = false;
                }
                else
                {
                    rn.material.color = originalColour;
                    transform.GetChild(0).gameObject.layer = PlatformLayer;
                }
            }

        }
    }
}
