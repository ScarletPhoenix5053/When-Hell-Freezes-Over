using UnityEngine;
using Sierra.Combat2D;

public class SlimePriestController : EnemyController, IBossAttackTriggerResponder
{
    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Standing, Aiming
    }
    public float AimRange;
    public float AimMinHeight;

    public void StartAttack(BossAttackPath attackPath)
    {
        throw new System.NotImplementedException();
    }

    protected override void Act()
    {
        // If aiming

        // aim
        throw new System.NotImplementedException();
    }
    protected override void DecideAction()
    {
        // If player in aim range

        // start aiming

        // else return to standing

        throw new System.NotImplementedException();
    }
}
