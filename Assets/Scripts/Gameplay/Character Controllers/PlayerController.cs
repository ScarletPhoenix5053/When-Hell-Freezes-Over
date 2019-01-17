using UnityEngine;
using System;
using System.Collections;
using Sierra.Combat2D;

[RequireComponent(typeof(CharacterMotionController))]
[RequireComponent(typeof(PlayerAttackManager))]
[RequireComponent(typeof(Health))]
public class PlayerController : BaseController
{
    #region Public Variables
    public float JumpHeight = 12f;
    public int AdditionalJumps = 1;

    public int RollFrames = 45;    

    public Action CurrentAction = Action.None;
    public enum Action { None, Attacking, Rolling, Climbing }

    public int Sign { get { if (mc.MoveVector.x == 0) return sign; else return sign = Math.Sign(mc.MoveVector.x); } }

    public Canvas TempDeathCanvas;
    #endregion
    #region Private Variables
    private PlayerAttackManager am;
    private PlayerAnimationController an;
    private Health hp;

    private InputData currentInputData;
    private IEnumerator currentRollRoutine;

    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;
    private int additionalJumpsUsed = 0;

    private int sign = 1;
    #endregion

    #region Unity Runtime Events
    protected override void Awake()
    {
        base.Awake();
        am = GetComponent<PlayerAttackManager>();
        an = GetComponent<PlayerAnimationController>();

        hp = GetComponent<Health>();

        if (TempDeathCanvas == null)
            throw new NullReferenceException("Please assign an object to TempDeathCanvas");
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.HitStopActive) return;

        if (CurrentState == State.Ready ||
            (CurrentState == State.InAction && CurrentAction == Action.Attacking))
            CheckInputByKeyCode();

        OrientByMotion();
        an.RunAnimationStateMachine();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.HitStopActive) return;

        if (CurrentAction == Action.Rolling) mc.MoveVector = new Vector2(Math.Sign(transform.localScale.x), 0);

        IncrimentJumpTimer();
        mc.UpdatePosition();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        currentInputData = data;
        Debug.LogWarning("ReadInput method disabled");
        //if (CurrentState == State.Ready && CurrentAction == Action.None) CheckInputAsNormal();
    }
    /// <summary>
    /// Change the player's current action state
    /// </summary>
    /// <param name="newAction"></param>
    public void SetAction(Action newAction)
    {
        if (CurrentAction == newAction) return;

        //Debug.Log("Changing player action from " + CurrentAction + " to " + newAction);
        CurrentAction = newAction;
    }
    public override void SetState(State newState)
    {
        if (newState == State.Ready) SetAction(Action.None);
        base.SetState(newState);
    }
    /// <summary>
    /// Perform death actions for this character.
    /// </summary>
    public override void Die()
    {
        SetState(State.Dead);

        // death anim

        // deactivate hurtbox
        foreach (Hurtbox hurtbox in GetComponent<Health>().Hurtboxes)
        {
            hurtbox.SetInactive();
        }

        // display you died message
        TempDeathCanvas.gameObject.SetActive(true);

        // restart game after delay
        GameManager.Instance.ReloadGame(3f);
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Makes the player face x input direction
    /// </summary>
    private void OrientByMotion()
    {
        transform.localScale = new Vector3(Sign, transform.localScale.y, transform.localScale.z);
    }    
    /// <summary>
    /// Performs input checks as if the character is unaffected by anything.
    /// </summary>
    private void CheckInputByKeyCode()
    {
        // Reset additionalJumps if on ground
        if (additionalJumpsUsed != 0 && mc.IsGrounded)
        {
            additionalJumpsUsed = 0;
        }

        if (mc.IsGrounded)
        {
            // Light attack button
            if (Input.GetKeyDown(KeyCode.J) && 
                (CurrentAction == Action.Attacking || CurrentAction == Action.None))
            {
                SetState(State.InAction);
                SetAction(Action.Attacking);
                am.NormalAttack();
            }
            // Dodge roll
            else if (Input.GetKeyDown(KeyCode.L) && CurrentAction == Action.None)
            {
                if (currentRollRoutine != null) StopCoroutine(currentRollRoutine);
                currentRollRoutine = RollRoutine();
                StartCoroutine(currentRollRoutine);
            }
        }

        // Ranged attack button
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetState(State.InAction);
            SetAction(Action.Attacking);
            am.RangedAttack();
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (mc.IsGrounded)
            {
                mc.DoImpulse(new Vector2(0, JumpHeight));
            }
            else if (additionalJumpsUsed < AdditionalJumps)
            {
                additionalJumpsUsed++;
                mc.DoImpulse(new Vector2(0, JumpHeight));
            }

            jumpLimitTimer = jumpLimitSeconds;
        }
        // Platform interactions
        if (Input.GetKey(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(9, 13, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 13, false);
        }

        if (CurrentAction == Action.None)
        {
            // Walk
            var walkAxis = 0;
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) { }
            else if (Input.GetKey(KeyCode.A)) walkAxis = -1;
            else if (Input.GetKey(KeyCode.D)) walkAxis = 1;
            if (walkAxis != 0)
            {
                mc.MoveVector = new Vector2(walkAxis, 0);
            }
        }
    }

    private void IncrimentJumpTimer()
    {
        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }
    private IEnumerator RollRoutine()
    {
        SetState(State.InAction);
        SetAction(Action.Rolling);
        foreach (Hurtbox hurtbox in hp.Hurtboxes)
        {
            hurtbox.SetInactive();
        }
        Physics2D.IgnoreLayerCollision(9, 10, true);        
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(RollFrames));
        yield return GameManager.Instance.UntillHitStopInactive();

        SetState(State.Ready);
        SetAction(Action.None);
        foreach (Hurtbox hurtbox in hp.Hurtboxes)
        {
            hurtbox.SetActive();
        }
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }
    #endregion
}