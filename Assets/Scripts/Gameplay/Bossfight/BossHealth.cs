using UnityEngine;
using System.Collections;

public class BossHealth : Health
{
    public int IFrames = 30;
    [ReadOnly]
    public bool Invulnerable;

    private IEnumerator currentRoutine;

    #region Public Methods
    public void StartIFrames()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = IFrameRoutine();
        StartCoroutine(currentRoutine);
    }
    public void DealDamageBoss()
    {
        if (AlreadyDead()) return;

        Hp -= 1;
        StartIFrames();

        Events.OnDamage.Invoke();

        if (Dead) Die();
    }

    public override void DealDamageArmored(AttackData data)
    {
        DealDamageBoss();
    }
    public override void DealDamageCritical(AttackData data)
    {
        DealDamageBoss();
    }
    public override void DealDamageNormal(AttackData data)
    {
        DealDamageBoss();
    }
    #endregion
    private IEnumerator IFrameRoutine()
    {
        Invulnerable = true;

        var timer = 0;
        while (timer < IFrames)
        {
            timer++;
            yield return new WaitForFixedUpdate();
        }

        Invulnerable = false;
    }
}
