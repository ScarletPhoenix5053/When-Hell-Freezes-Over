using System;
using UnityEngine;
using UnityEngine.Events;
using Sierra.Combat2D;

/// <summary>
/// Controller for ranged projeciles.
/// </summary>
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(MotionController))]
public class ProjectileController : MonoBehaviour, IHitboxResponder
{
    public ProjectileEvents Events;
    [Serializable]
    public class ProjectileEvents
    {
        public UnityEvent OnHit;
        public UnityEvent OnEnvironmentImpact;
    }    

    protected int ySign = 1;
    protected int xSign = 1;

    protected Hitbox hb;
    protected MotionController mc;
    protected AttackManager orign;

    public AttackData attackData;

    protected void Awake()
    {
        hb = GetComponent<Hitbox>();
        mc = GetComponent<MotionController>();
        SetHitboxResponder(this);
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
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Events.OnEnvironmentImpact.Invoke();
        Destroy(gameObject);
    }

    // IHitboxResponder
    public virtual void Hit(Collider2D hurtbox)
    {
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.        
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb != null)
        {
            // if hit a non-player hurtbox
            if (hb is ButtonHurtbox || hb is BreakableWall)
            {
                Events.OnEnvironmentImpact.Invoke();
            }
            // else must have hit a character
            else
            {
                var hurtState = hb.GetHurtboxState();
                switch (hurtState)
                {
                    case Hurtbox.State.Inactive:
                        Debug.Log("Hit inactive hurtbox");
                        return;

                    case Hurtbox.State.Vulnerable:
                        ApplyAttackDamageTo(hurtbox, hurtState);
                        break;

                    case Hurtbox.State.Critical:
                        ApplyAttackDamageTo(hurtbox, hurtState);
                        break;

                    case Hurtbox.State.Blocking:
                        Debug.Log("Attack was blocked");
                        break;

                    default:
                        return;
                }
                Events.OnHit.Invoke();
            }
        }
        Destroy(gameObject);
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

    /// <summary>
    /// Deal damage to a hit hurtbox. Do not call this method outside of <see cref="Hit(Collider2D)"/>
    /// </summary>
    /// <param name="hurtbox"></param>
    /// <param name="hurtState"></param>
    protected void ApplyAttackDamageTo(Collider2D hurtbox, Hurtbox.State hurtState)
    {
        // Initialize
        var hp = hurtbox.GetComponent<Hurtbox>().hp;
        attackData.Sign = xSign;

        // Deal Damage
        if (hurtState == Hurtbox.State.Critical) hp.DealDamageCritical(attackData);
        else if (hurtState == Hurtbox.State.Armored) hp.DealDamageArmored(attackData);
        else hp.DealDamageNormal(attackData);

        // Destroy self on impact
        Destroy(gameObject);
    }
}
