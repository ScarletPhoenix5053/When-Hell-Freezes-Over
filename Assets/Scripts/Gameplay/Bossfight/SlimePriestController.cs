using UnityEngine;
using UnityEditor;

public class SlimePriestController : EnemyController, IBossAttackTriggerResponder
{
    #region Public Vars
    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Standing, Aiming
    }
    public float AimRange;
    public float AimMinHeight;

    public Transform SpearArm;
    #endregion
    #region Protected Vars
    protected Vector2 spearArmOriginalPos;
    protected Quaternion spearArmOriginalRot;
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();

        spearArmOriginalPos = SpearArm.position;
        spearArmOriginalRot = SpearArm.rotation;
    }
    protected override void FixedUpdate()
    {
        if (CurrentState == State.Ready)
        {
            DecideAction();
            Act();
        }
    }
    protected void OnDrawGizmosSelected()
    {
        DrawCircle(AimRange, Color.cyan);
    }
    #endregion

    #region Public Methods
    public void StartAttack(BossAttackPath attackPath)
    {
        throw new System.NotImplementedException();
    }
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (CurrentBehaviour != newBehaviour) CurrentBehaviour = newBehaviour;
    }
    #endregion
    #region Protected Methods
    protected override void Act()
    {
        // If aiming
        if (CurrentBehaviour == Behaviour.Aiming)
        {
            // aim
            SpearArm.LookAt(plr.transform);
        }
        else
        {
            SpearArm.position = spearArmOriginalPos;
            SpearArm.rotation = spearArmOriginalRot;
        }
    }
    protected override void DecideAction()
    {
        // If player in aim range
        if (DistToPlayer < AimRange && plr.transform.position.y >= AimMinHeight)
        {
            // start aiming
            SetBehaviour(Behaviour.Aiming);
        }
        else
        {
            // return to standing
            SetBehaviour(Behaviour.Standing);
        }
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    #endregion
}