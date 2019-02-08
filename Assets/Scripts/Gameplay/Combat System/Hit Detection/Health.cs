using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.UI;
using Sierra;
using Sierra.Combat2D;

public class Health : MonoBehaviour
{
    #region Public Variables
    public bool isPlayer { get { return GetComponent<BaseController>() is PlayerController; } }

    public int HpMax = 6;
    public int Hp = 6;
    public float SuperStunMultiplier = 3f;

    private int StartHearts = 3;
    private int HealthPerHeart = 2;
    private int MaxHeartAmount = 3;

    public Image[] HealthImages;
    public Sprite[] HealthSprites;

    public bool AffectedByKnockback = true;
    public bool AffectedByKnockbackOnCrit = true;

    public Hurtbox Hurtbox;

    public EnemyEvents Events;
    [Serializable]

    public class EnemyEvents
    {
        public UnityEvent OnDamage;
        public UnityEvent OnDeath;
        public UnityEvent OnCriticalHit;
        public UnityEvent OnArmoredHit;
        public UnityEvent OnRecovery;
    }
    public bool Dead { get { return Hp <= 0; } }
    #endregion
    #region Private Variables
    private bool loggedHeartWarning;

    private CharacterMotionController mc;
    private BaseController chr;

    private AttackData atkData;
    private IEnumerator currentHsRoutine;
    #endregion

    private void Awake()
    {
        chr = GetComponent<BaseController>();
        mc = GetComponent<CharacterMotionController>();
        Hurtbox = GetComponent<Hurtbox>();
    }
    private void Start()
    {
        if (isPlayer)
        {
            Hp = StartHearts * HealthPerHeart;
            HpMax = MaxHeartAmount * HealthPerHeart;

            CheckHealthAmount();
        }
    }
    private void Update()
    {
        UpdateHeartImages();
    }

    #region Public Methods
    public virtual void DealDamageNormal(AttackData data)
    {
        if (AlreadyDead()) return;

        atkData = data;
        AdjustHP();
        UpdateHearts();

        // Check if died this frame
        if (Dead) Die();
        else
        {
            ApplyHitStun();
            ApplyKnockBack();
            Events.OnDamage.Invoke();
        }
    }
    public virtual void DealDamageCritical(AttackData data)
    {
        if (AlreadyDead()) return;
        
        atkData = data;
        AdjustHP();
        UpdateHearts();

        // Check if died this frame
        if (Dead) Die();
        else
        {
            ApplySuperStun();
            ApplyKnockBack();
            Events.OnCriticalHit.Invoke();
        }
    }
    public virtual void DealDamageArmored(AttackData data)
    {
        if (AlreadyDead()) return;

        Debug.Log(name + "was damaged - armored!");
        atkData = data;
        AdjustHP();
        UpdateHearts();

        // Check if died this frame
        if (Dead) Die();
        else
        {
            Events.OnArmoredHit.Invoke();
        }
    }

    public virtual void Die()
    {
        Hp = 0;
        if (chr is EnemyController)
        {
            var enm = chr as EnemyController;
            Events.OnDeath.Invoke();
        }
        StopAllCoroutines();
    }
    public void LogHp()
    {
        Debug.Log(name + ": " + Hp + "/ " + HpMax);
    }
    public void LogDeath()
    {
        Debug.Log(name + " is dead");
    }
    public void UpdateHearts()
    {

        if (isPlayer)
        {
            bool empty = false;
            int i = 0;

            foreach (Image image in HealthImages)
            {

                if (empty)
                {
                    image.sprite = HealthSprites[0];
                }
                else
                {
                    i++;
                    if (Hp >= i * HealthPerHeart)
                    {
                        image.sprite = HealthSprites[HealthSprites.Length - 1];
                    }
                    else
                    {
                        int currentHeartHealth = (int)(HealthPerHeart - (HealthPerHeart * i - Hp));
                        int healthPerImage = HealthPerHeart / (HealthSprites.Length - 1);
                        int imageIndex = currentHeartHealth / healthPerImage;
                        image.sprite = HealthSprites[imageIndex];
                        empty = true;
                    }
                }
            }
        }
    }
    public void CheckHealthAmount()
    {
        if (HealthImages == null) return;
        if (HealthImages.Length < MaxHeartAmount) return;

        if (isPlayer)
        {
            for (int i = 0; i < MaxHeartAmount; i++)
            {
                if (StartHearts <= i)
                {
                    HealthImages[i].enabled = false;
                }
                else
                {
                    HealthImages[i].enabled = true;
                }
            }

            UpdateHearts();
        }
    }
    public void TakeDamage(int amount)
    {
        Hp -= amount;
        Hp = Mathf.Clamp(Hp, 0, StartHearts * HealthPerHeart);
        UpdateHearts();
    }
    #endregion
    #region Private Methods
    protected void AdjustHP()
    {

        if (atkData.Damage != 0)
        {
            // Use "Armore Factor" instead? (character controllers carry a multiplier for incoming damage)
            if (chr?.CurrentState == BaseController.State.SuperStun)
            {
                Hp -= Convert.ToInt32(atkData.Damage * SuperStunMultiplier);
            }
            else
            {
                Hp -= atkData.Damage;
                UpdateHearts();
            }
        }
    }
    private void ApplyHitStun()
    {
        // Exit early if in superstun to prevent normal hitstun from overriding.
        if (chr?.CurrentState == BaseController.State.SuperStun) return;

        Debug.Log("HS");
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = HitStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplySuperStun()
    {
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = SuperStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplyKnockBack()
    {
        if (!AffectedByKnockback) return;
        if (Hurtbox.CurrentState == Hurtbox.State.Critical && !AffectedByKnockbackOnCrit) return;

        var sign = Mathf.Sign(transform.localScale.x);
        mc?.DoImpulse(new Vector2(atkData.KnockBack * atkData.Sign, atkData.KnockUp));
    }
    private void UpdateHeartImages()
    {
        if (HealthImages == null) return;
        if (HealthImages.Length < 3) return;

        if (isPlayer && Hp == HpMax)
        {
            HealthImages[0].sprite = HealthSprites[2];
            HealthImages[1].sprite = HealthSprites[2];
            HealthImages[2].sprite = HealthSprites[2];
        }
    }
    /// <summary>
    /// Log warning and return if ALREADY dead
    /// </summary>
    protected bool AlreadyDead()
    {
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return true;
        }
        else return false;
    }

    private IEnumerator HitStunRoutine()
    {
        chr?.SetState(BaseController.State.HitStun);

        // Start timer
        yield return Utility.FrameTimer(atkData.HitStun, 0);

        // End timer
        chr?.SetState(BaseController.State.Ready);
    }
    private IEnumerator SuperStunRoutine()
    {
        Debug.Log("Applying Superstun for 3.5 seconds");
        chr?.SetState(BaseController.State.SuperStun);
        Hurtbox.SetState(Hurtbox.State.Vulnerable);

        // Start timer
        yield return Utility.FrameTimer(210, 0);

        // End timer
        chr?.SetState(BaseController.State.Ready);
        Events.OnRecovery.Invoke();

        Debug.Log("Superstun end");
    }    
    #endregion
}