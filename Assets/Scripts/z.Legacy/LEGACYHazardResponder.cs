using UnityEngine;
using System;
using Sierra;
using Sierra.Combat2D;
using System.Collections;

public class LEGACYHazardResponder : MonoBehaviour, IHitboxResponder
{
    public AttackData DamageData;
    public Hitbox Hitbox;
    public bool Active { get; private set; }

    private void Awake()
    {
        if (DamageData == null) throw new NullReferenceException("Please assign DamageData");
        if (Hitbox == null) throw new NullReferenceException("Please assign hazard's Hitbox");
    }
    public void OnEnable()
    {
        Hitbox.SetResponder(this);
        Hitbox.SetActive();
    }
    public void OnDisable()
    {
        Hitbox.SetInactive();
    }
    private void FixedUpdate()
    {
        Hitbox.UpdateHitbox();
    }

    
    public void Hit(Collider2D hurtbox)
    {
        var hb = hurtbox.GetComponent<Hurtbox>();
        if (hb != null)
        {
            if (hb.CheckHit())
            {
                DamageData.Sign = Math.Sign(transform.localScale.x);
                hurtbox.GetComponent<Hurtbox>().hp.Damage(DamageData);

                GameManager.Instance.HitStopFor(DamageData.HitStop);

                StartCoroutine(ReactivationRoutine());
            }
        }
    }
    public IEnumerator ReactivationRoutine()
    {
        Hitbox.SetInactive();
        yield return new WaitForSeconds(Utility.FramesToSeconds(DamageData.Recovery));

        Hitbox.SetResponder(this);
        Hitbox.SetActive();
    }
}
