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

    public override int Sign { get { if (mc.MoveVector.x == 0) return sign; else return sign = Math.Sign(mc.MoveVector.x); } }

    public Canvas TempDeathCanvas;
    #endregion
    #region Private Variables
    private PlayerAttackManager am;
    private PlayerAnimationController an;
    private Health hp;
    
    private IEnumerator currentRollRoutine;

    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;
    private int additionalJumpsUsed = 0;
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
            (CurrentState == State.Action && CurrentAction == Action.Attacking))
            CheckInput();

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
        
        GetComponent<Health>().Hurtbox.SetInactive();

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
    private void CheckInput()
    {
        // Reset additionalJumps if on ground
        if (additionalJumpsUsed != 0 && mc.IsGrounded)
        {
            additionalJumpsUsed = 0;
        }

        if (mc.IsGrounded)
        {
            // Light attack button
            if (InputManager.Attack() && 
                (CurrentAction == Action.Attacking || CurrentAction == Action.None))
            {
                SetState(State.Action);
                SetAction(Action.Attacking);
                am.NormalAttack();
            }
            // Dodge roll
            else if (InputManager.Roll() && CurrentAction == Action.None)
            {
                if (currentRollRoutine != null) StopCoroutine(currentRollRoutine);
                currentRollRoutine = RollRoutine();
                StartCoroutine(currentRollRoutine);
            }
        }

        // Ranged attack button
        if (InputManager.RangedAttack())
        {
            SetState(State.Action);
            SetAction(Action.Attacking);
            am.RangedAttack();
        }

        // Jump
        if (InputManager.Jump())
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
        if (InputManager.HoldingDown())
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
            if (InputManager.MotionAxis() != 0)
            {
                mc.MoveVector = new Vector2(InputManager.MotionAxis(), 0);
            }
        }
    }

    private void IncrimentJumpTimer()
    {
        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }
    private IEnumerator RollRoutine()
    {
        var capsule = GetComponent<CapsuleCollider2D>();
        SetState(State.Action);
        SetAction(Action.Rolling);
        capsule.size = new Vector2(capsule.size.x, capsule.size.y / 2);
        capsule.offset = new Vector2(capsule.offset.x, capsule.offset.y - 0.6f);
        hp.Hurtbox.SetInactive();

        Physics2D.IgnoreLayerCollision(9, 10, true);        
        yield return new WaitForSeconds(Sierra.Utility.FramesToSeconds(RollFrames));
        yield return GameManager.Instance.UntillHitStopInactive();

        SetState(State.Ready);
        SetAction(Action.None);
        capsule.size = new Vector2(capsule.size.x, capsule.size. y * 2);
        capsule.offset = new Vector2(capsule.offset.x, capsule.offset.y + 0.6f);
        hp.Hurtbox.SetActive();

        Physics2D.IgnoreLayerCollision(9, 10, false);
    }
    #endregion
}