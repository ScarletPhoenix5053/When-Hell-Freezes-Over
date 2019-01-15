using System.Collections;
using System;
using UnityEngine;
using Sierra.Combat2D;

/// <summary>
/// Controller for ranged projeciles. Only allows travel along the X axis for now!
/// </summary>
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(MotionController))]
public class ProjectileController : MonoBehaviour
{
    protected int sign = 1;
    protected Hitbox hb;
    protected MotionController mc;

    protected void Awake()
    {
        Debug.Log("Awake");
        hb = GetComponent<Hitbox>();
        mc = GetComponent<MotionController>();
    }
    protected void OnEnable()
    {
        hb.SetActive();
    }
    protected void FixedUpdate()
    {
        mc.MoveVector = new Vector2(sign, 0);
        mc.UpdatePosition();
    }

    /// <summary>
    /// Sign controls which diection the projecile travels in.
    /// </summary>
    /// <param name="newSign"></param>
    public void SetSign(int newSign)
    {
        sign = Math.Sign(newSign);
    }
    /// <summary>
    /// Assign <see cref="Hitbox.responder"/> for all attatched hitboxes.
    /// </summary>
    /// <param name="responder"></param>
    public void SetHitboxResponder(IHitboxResponder responder)
    {
        Debug.Log("SetResponder");
        if (hb == null) hb = GetComponent<Hitbox>();
        hb.SetResponder(responder);
    }
}
