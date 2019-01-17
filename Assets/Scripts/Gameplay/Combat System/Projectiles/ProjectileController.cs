using System.Collections;
using System;
using UnityEngine;
using Sierra.Combat2D;

/// <summary>
/// Controller for ranged projeciles.
/// </summary>
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(MotionController))]
public class ProjectileController : MonoBehaviour, IHitboxResponder
{
    protected int ySign = 1;
    protected int xSign = 1;

    protected Hitbox hb;
    protected MotionController mc;
    protected AttackManager orign;

    protected AttackData attackData;

    protected void Awake()
    {
        hb = GetComponent<Hitbox>();
        mc = GetComponent<MotionController>();
    }
    protected void OnEnable()
    {
        hb.SetActive();
    }
    protected void FixedUpdate()
    {
        hb.UpdateHitbox();

        mc.MoveVector = new Vector2(xSign, ySign);
        mc.UpdatePosition();
    }

    // IHitboxResponder
    public virtual void Hit(Collider2D hurtbox)
    {
        Debug.Log("Called a hit");
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.        
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb != null)
        {
            // if hit a button
            if (hb is ButtonHurtbox)
            {
                Debug.Log("Hit a button");
                hb.CheckHit();
            }
            // else must have hit a character
            else if (hb.CheckHit())
            {
                Debug.Log("Hit a tihng");

                // set sign of attack
                attackData.Sign = xSign;
                hurtbox.GetComponent<Hurtbox>().hp.Damage(attackData);
                //hurtbox.GetComponent<Hurtbox>().Health.LogHp();

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Sign controls which diection the projecile travels in. Processes value so there's no neet to convert it beforehand.
    /// </summary>
    /// <param name="newSign"></param>
    public void SetSign(int newSign)
    {
        xSign = Math.Sign(newSign);
    }
    /// <summary>
    /// Assign <see cref="Hitbox.responder"/> for all attatched hitboxes.
    /// </summary>
    /// <param name="responder"></param>
    public void SetHitboxResponder(IHitboxResponder responder)
    {
        if (hb == null) hb = GetComponent<Hitbox>();
        hb.SetResponder(responder);
    }
    public void SetAttackData(AttackData newData)
    {
        attackData = newData;
    }
}
