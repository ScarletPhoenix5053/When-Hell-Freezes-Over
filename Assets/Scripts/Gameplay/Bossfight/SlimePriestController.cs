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
    public BossAttackTrigger AttackTrigger;
    
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
        AttackTrigger.SetResponder(this);
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
    }
    #endregion

    #region Public Methods
    public void StartAttack()
    {
        GenericEvents.OnAttack.Invoke();
        am?.Attack();
    }
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (CurrentBehaviour != newBehaviour) CurrentBehaviour = newBehaviour;
    }
    public override void Die()
    {
        Debug.Log(name + "Is Dead.");
        SetState(State.Dead);
        am?.StopAttack();

        // Despawn
        if (transform.parent != null) Destroy(transform.parent.gameObject, 2.7f);
        else Destroy(gameObject, 2.7f);
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