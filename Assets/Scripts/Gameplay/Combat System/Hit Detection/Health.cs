using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Sierra;
using Sierra.Combat2D;

[RequireComponent(typeof(CharacterMotionController))]
public class Health : MonoBehaviour
{
    #region Public Variables
    public bool isPlayer;

    public int HpMax = 6;
    public int Hp = 6;
    public float SuperStunMultiplier = 3f;

    private int startHearts = 3;
    private int healthPerHeart = 2;
    private int maxHeartAmount = 3;

    public Image[] healthImages;
    public Sprite[] healthSprites;

    public bool AffectedByKnockback = true;
    public bool AffectedByKnockbackOnCrit = true;

    public Hurtbox Hurtbox;

    public bool Dead { get { return Hp <= 0; } }
    #endregion
    #region Private Variables
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
            Hp = startHearts * healthPerHeart;
            HpMax = maxHeartAmount * healthPerHeart;

            CheckHealthAmount();
        }
    }

    private void Update()
    {
        if (isPlayer && Hp == HpMax)
        {
            healthImages[0].sprite = healthSprites[2];
            healthImages[1].sprite = healthSprites[2];
            healthImages[2].sprite = healthSprites[2];
        }
    }
    #region Public Methods
    public void Damage(AttackData data, bool critical = false)
    {
        Debug.Log(name + "was damaged");

        // Log warning and return if ALREADY dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        atkData = data;
        AdjustHP();
        UpdateHearts();

        // Check if died this frame
        if (Dead) Die();
        else
        {
            // Check for critical hit
            if (critical)
            {
                ApplySuperStun();
                ApplyKnockBack();
            }
            else
            {
                ApplyHitStun();
                ApplyKnockBack();
            }
        }
    }

    public void LogHp()
    {
        Debug.Log(name + ": " + Hp + "/ " + HpMax);
    }
    public void LogDeath()
    {
        Debug.Log(name + " is dead");
    }
    #endregion
    #region Private Methods
    private void AdjustHP()
    {

        if (atkData.Damage != 0)
        {
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
        LogHp();
    }
    private void ApplyHitStun()
    {
        // Exit early if in superstun to prevent normal hitstun from overriding.
        if (chr?.CurrentState == BaseController.State.SuperStun) return;

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
    private void Die()
    {
        Hp = 0;
        chr?.Die();
        StopAllCoroutines();
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

        // Start timer
        yield return Utility.FrameTimer(210, 0);

        // End timer
        chr?.SetState(BaseController.State.Ready);
    }
    public void CheckHealthAmount()
    {
        if (isPlayer)
        {
            for (int i = 0; i < maxHeartAmount; i++)
            {
                if (startHearts <= i)
                {
                    healthImages[i].enabled = false;
                }
                else
                {
                    healthImages[i].enabled = true;
                }
            }

            UpdateHearts();
        }
    }

    public void UpdateHearts()
    {
        if (isPlayer)
        {
            bool empty = false;
            int i = 0;

            foreach (Image image in healthImages)
            {

                if (empty)
                {
                    image.sprite = healthSprites[0];
                }
                else
                {
                    i++;
                    if (Hp >= i * healthPerHeart)
                    {
                        image.sprite = healthSprites[healthSprites.Length - 1];
                    }
                    else
                    {
                        int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart * i - Hp));
                        int healthPerImage = healthPerHeart / (healthSprites.Length - 1);
                        int imageIndex = currentHeartHealth / healthPerImage;
                        image.sprite = healthSprites[imageIndex];
                        empty = true;
                    }
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        Hp -= amount;
        Hp = Mathf.Clamp(Hp, 0, startHearts * healthPerHeart);
        UpdateHearts();
    }
    #endregion
}