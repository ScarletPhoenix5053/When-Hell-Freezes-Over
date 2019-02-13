using Sierra;
using Sierra.Combat2D;
using System.Collections;
using UnityEngine;

public class ShieldBlobController : EnemyController
{
    public float PushRange = 1f;
    public int TurnDuration = 120;
    public GameObject BouncePadPrefab;

    public GameObject bone, eyeball;//Put this in every enemy with what items they drop. Since we only have 3 enemies it's not bad.
    public Vector2 AverageItemVariance = new Vector2(3, 3);

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Guarding, ReadyToPush, Pushing
    }

    protected IEnumerator currentTurnRoutine;
    protected bool turning;
    protected int turningTo;

    protected override void Awake()
    {
        base.Awake();
    }
    protected void OnDrawGizmosSelected()
    {
        Utility.DrawCircle(transform.position, PushRange, Color.red);
    }

    public void SetBehaviour(Behaviour newBehaviour)
    {
        CurrentBehaviour = newBehaviour;
    }
    public void StopCurrentTurnRoutine()
    {
        if (currentTurnRoutine == null)
        {
            return;
        }

        StopCoroutine(currentTurnRoutine);
        turning = false;
    }

    protected override void Act()
    {
        // Turn to face player after delay on sideswitch
        if ((Sign == -1 && !PlayerToLeft) ||
            (Sign == 1 && PlayerToLeft))
            {
            if (!turning)
            {
                StopCurrentTurnRoutine();
                currentTurnRoutine = TurnRoutine();
                StartCoroutine(currentTurnRoutine);
            }
            }

        //  Block if facing player, be critical if not
        if ((Sign == -1 && PlayerToLeft) ||
            (Sign == 1 && !PlayerToLeft))
        {
            hp.Hurtbox.SetState(Hurtbox.State.Blocking);
        }
        else
        {
            hp.Hurtbox.SetState(Hurtbox.State.Critical);
        }

        // READYTOPUSH
        // Push player if facing them
        if (CurrentBehaviour == Behaviour.ReadyToPush &&
            CurrentState == State.Ready &&
            (
            Sign == -1 && PlayerToLeft ||
            Sign == 1 && !PlayerToLeft
            ))
        {
            var e_am = am as EnemyAttackManager;
            e_am.Attack();

            GenericEvents.OnAttack.Invoke();
            CurrentState = State.Action;
        }
    }
    protected override void DecideAction()
    {
        // Switch to push anticipation state when player is very close
        // Switch back to guarding if player is no longer close      

        switch (CurrentBehaviour)
        {
            case Behaviour.Guarding:
                if (DistToPlayer < PushRange) SetBehaviour(Behaviour.ReadyToPush);
                break;

            case Behaviour.ReadyToPush:
                if (DistToPlayer > PushRange) SetBehaviour(Behaviour.Guarding);
                break;

            default:
                break;
        }
    }
    public override void Die()
    {
        int lootNum = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < lootNum; i++)
        {
            var boneVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var boneVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var eyeVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var eyeVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var boneSpawnPos = new Vector3(
                transform.position.x + boneVarianceX,
                transform.position.y + boneVarianceY,
                transform.position.z);

            var eyeSpawnPos = new Vector3(
                transform.position.x + eyeVarianceX,
                transform.position.y + eyeVarianceY,
                transform.position.z);

            Instantiate(bone, boneSpawnPos, transform.rotation);
            Instantiate(eyeball, eyeSpawnPos, transform.rotation);
        }

        if (BouncePadPrefab != null)
            Instantiate(
                BouncePadPrefab,
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);
        base.Die();
    }

    protected IEnumerator TurnRoutine()
    {
        var turnTimer = 0;

        turning = true;
        yield return Utility.FrameTimer(TurnDuration, turnTimer);

        turning = false;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
