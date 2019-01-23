using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Sierra.Combat2D;

[RequireComponent(typeof(CharacterMotionController))]
public class Health : MonoBehaviour
{
    public int Hp;
    public int HpMax; // = 6;

    private int startHearts = 3;
    private int healthPerHeart = 2;
    private int maxHeartAmount = 3;

    public Image[] healthImages;
    public Sprite[] healthSprites;

    public Hurtbox[] Hurtboxes;

    public bool Dead { get { return Hp <= 0; } }

    private CharacterMotionController mc;
    private BaseController chr;

    private AttackData atkData;
    private IEnumerator currentKbRoutine;
    private IEnumerator currentHsRoutine;

    private void Awake()
    {
        chr = GetComponent<BaseController>();
        mc = GetComponent<CharacterMotionController>();
    }

    private void Start()
    {
        Hp = startHearts * healthPerHeart;
        HpMax = maxHeartAmount * healthPerHeart;

        CheckHealthAmount();
    }

    private void Update()
    {
        if (Hp == HpMax)
        {
            healthImages[0].sprite = healthSprites[2];
            healthImages[1].sprite = healthSprites[2];
            healthImages[2].sprite = healthSprites[2];
        }
    }

    public void Damage(AttackData data)
    {
        atkData = data;
        UpdateHearts();
        // Log warning and return if ALREADY dead
        if (Dead)
        {
            Debug.LogWarning(name + " is already dead");
            return;
        }

        // Adjust Hp
        if (data.Damage != 0) Hp -= data.Damage;

        // Check if died this frame
        if (Hp <= 0)
        {
            Hp = 0;
            chr.Die();
            StopAllCoroutines();
        }
        else
        {
            // Apply Knockback/Stun
            ApplyHitStun();
            ApplyKnockBack();
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

    private void ApplyHitStun()
    {
        if (currentHsRoutine != null) StopCoroutine(currentHsRoutine);
        currentHsRoutine = HitStunRoutine();
        StartCoroutine(currentHsRoutine);
    }
    private void ApplyKnockBack()
    {
        var sign = Mathf.Sign(transform.localScale.x);
        mc?.DoImpulse(new Vector2(atkData.KnockBack * atkData.Sign, atkData.KnockUp));
    }
    private IEnumerator HitStunRoutine()
    {
        var chr = GetComponent<BaseController>();

        chr.SetState(BaseController.State.InHitstun);
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(atkData.HitStun));
        yield return GameManager.Instance.UntillHitStopInactive();

        chr.SetState(BaseController.State.Ready);
    }


    public void CheckHealthAmount()
    {

        for (int i = 0; i < maxHeartAmount; i++)
        {
            if(startHearts <= i)
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

    public void UpdateHearts()
    {
        bool empty = false;
        int i = 0;

        foreach(Image image in healthImages)
        {

            if (empty)
            {
                image.sprite = healthSprites[0];
            }
            else
            {
                i++;
                if(Hp >= i * healthPerHeart)
                {
                    image.sprite = healthSprites[healthSprites.Length - 1];
                }
                else
                {
                    int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart*i - Hp));
                    int healthPerImage = healthPerHeart / (healthSprites.Length - 1);
                    int imageIndex = currentHeartHealth / healthPerImage;
                    image.sprite = healthSprites[imageIndex];
                    empty = true;
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
}