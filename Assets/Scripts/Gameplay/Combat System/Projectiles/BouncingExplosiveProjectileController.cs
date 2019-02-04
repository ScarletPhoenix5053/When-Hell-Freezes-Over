using System;
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
    [Range(0,1)]
    public float BounceDecayStrength = 0.2f;
    public float LinearDecay = 0.1f;
    public AttackData attackData;
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
        attackData = newData;
    }
    public virtual void Hit(Collider2D hurtbox)
    {
        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        // Disable hitbox on hit.        
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb is ButtonHurtbox) return;

        // SCRAP THIS: replace with explosion method
        if (hb.GetHurtboxState() != Hurtbox.State.Inactive &&
            hb.GetHurtboxState() != Hurtbox.State.Blocking
            )
        {
            // Will need to fix this later, bomb creates knockback away from its center
            attackData.Sign = 1;
            hb.hp.DealDamageNormal(attackData);
        }

        Events.OnHit.Invoke();
        Destroy(gameObject);
    }
    #endregion
    #region Private Methods
    private void Explode()
    {
        // find all hurtboxes in radius
        
        // identify which objects are on the left and right sides
        // apply knockback and damage to each group of objects
    }
    #endregion
}