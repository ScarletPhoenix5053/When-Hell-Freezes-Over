using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sierra.Combat2D;

/// <summary>
/// Controller for ranged projeciles. Explodes on impact with hurtbox, bounces on impact with environment.
/// </summary>
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterMotionController))]
public class BouncingExplosiveProjectileController : MonoBehaviour, IHitboxResponder
{
    #region Events
    public ProjectileEvents Events;
    [Serializable]
    public class ProjectileEvents
    {
        public UnityEvent OnHit;
        public UnityEvent OnEnvironmentImpact;
    }
    #endregion
    #region Public Vars
    public float BlastRadius = 2f;
    public float Fuse = 3f;
    [Range(0,1)]
    public float BounceDecayStrength = 0.2f;
    public float LinearDecay = 0.1f;
    public AttackData AttackData;
    #endregion
    #region Private Vars
    private CharacterMotionController mc;
    private Hitbox hb;

    private float downVel;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        mc = GetComponent<CharacterMotionController>();
        hb = GetComponent<Hitbox>();
    }
    private void Start()
    {
        hb.SetActive();
        hb.SetResponder(this);
    }
    private void FixedUpdate()
    {
        // Motion
        mc.MoveVector = Vector2.right;
        mc.UpdatePosition();

        if (mc.XSpeed > 0.05) mc.XSpeed -= LinearDecay;
        else mc.XSpeed = 0;

        // Hitbox
        hb.UpdateHitbox();

        // Fuse time
        if (Fuse <= 0)
        {
            Explode();
        }
        else
        {
            Fuse -= Time.fixedDeltaTime;
        }
    }
    private void LateUpdate()
    {
        downVel = mc.ContMotionVector.y;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Events.OnEnvironmentImpact.Invoke();

        // On collision with anything, bounce upwards
        mc.DoImpulse(new Vector2(mc.ContMotionVector.x, -(downVel * BounceDecayStrength)));
    }
    #endregion

    #region Public Methods
    public void SetAttackData(AttackData newData)
    {
        AttackData = newData;
    }
    public virtual void Hit(Collider2D hurtbox)
    {
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.        
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb is ButtonHurtbox) return;

        Events.OnHit.Invoke();
        Explode();

    }
    public void Explode()
    {
        Debug.Log("Boom");

        // find all hurtboxes in radius
        var overlaps = Physics2D.OverlapCircleAll(transform.position, BlastRadius);

        // identify which objects are on the left and right sides
        List<Hurtbox> left = new List<Hurtbox>();
        List<Hurtbox> right = new List<Hurtbox>();
        foreach (Collider2D overlap in overlaps)
        {
            Hurtbox hurtbox = overlap.GetComponent<Hurtbox>();
            if (overlap.transform.position.x <= transform.position.x)
            {
                if (hurtbox != null) left.Add(hurtbox);
            }
            else
            {
                if (hurtbox != null) right.Add(hurtbox);
            }
        }

        // apply knockback and damage to each group of objects
        if (left != null && left.Count > 0)
        {
            foreach (Hurtbox hurtbox in left)
            {
                AttackData.Sign = -1;
                hurtbox.hp.DealDamageNormal(AttackData);
            }
        }
        if (right != null && right.Count > 0)
        {
            foreach (Hurtbox hurtbox in right)
            {
                AttackData.Sign = 1;
                hurtbox.hp.DealDamageNormal(AttackData);
            }
        }
        Destroy(gameObject);
    }
    #endregion
}