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
    
    #endregion
    #region Protected Vars
    protected Vector2 spearArmOriginalPos;
    protected Quaternion spearArmOriginalRot;
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();
    }
    protected void Start()
    {
        FindObjectOfType<FightManager>()?.GoToNextStage();
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
    public void StartAttack()
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
    }
    protected override void DecideAction()
    {
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    #endregion
}