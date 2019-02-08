using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMotionController : MotionController
{
    #region Public Vars
    /// <summary>
    /// Linear gravity acceleration
    /// </summary>
    public float Gravity = 1f;
    /// <summary>
    /// Maximum gravity velocity.
    /// </summary>
    public float GravityMax = 20f;
    /// <summary>
    /// Linear drag deceletration on the X axis
    /// </summary>
    public float DragX = 0.5f;
    /// <summary>
    /// Returns true when touching the ground.
    /// </summary>
    public bool IsGrounded
    {
        get
        {
            return GroundedLeft || GroundedRight;
        }
    }
    /// <summary>
    /// Calls the Left-Side grounding ray.
    /// </summary>
    public bool GroundedLeft
    {
        get
        {
            return Physics2D.Raycast(
                new Vector2(
                    col.bounds.center.x - col.bounds.extents.x,
                    col.bounds.center.y - col.bounds.extents.y
                    ),
                Vector2.down,
                groundBuffer,
                GroundMask);
        }
    }
    /// <summary>
    /// Calls the Right-Side grounding ray.
    /// </summary>
    public bool GroundedRight
    {
        get
        {
            return Physics2D.Raycast(
                new Vector2(
                    col.bounds.center.x + col.bounds.extents.x,
                    col.bounds.center.y - col.bounds.extents.y
                    ),
                Vector2.down,
                groundBuffer,
                GroundMask);
        }
    }
    public bool GravityEnabledByDefault = true;
    public Vector2 ContMotionVector;

    public UnityEvent OnLanding;
    #endregion
    #region Protected Vars
    protected BaseController chr;
    protected Collider2D col;

    protected const float groundBuffer = 0.05f;
    protected const float groundStick = 0.1f;
    protected const float zeroThreshold = 0.05f;
    protected const int deltaMultiplicationFactor = 50;

    [ReadOnly][SerializeField]
    protected bool gravityEnabled = true;
    protected bool impulseLastFrame = false;
    protected bool inAirLastFrame = false;
    protected Vector2 combinedMotionVector;

    protected LayerMask GroundMask
    {
        get
        {
            LayerMask layerMask;
            if (InputManager.HoldingDown()) layerMask = LayerMask.GetMask("Environment");
            else layerMask = LayerMask.GetMask("Environment", "Platform");
            return layerMask;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        chr = GetComponent<BaseController>();
        col = GetComponent<Collider2D>();

        if (!GravityEnabledByDefault) gravityEnabled = false;
    }
    protected void Update()
    {
        if (inAirLastFrame && IsGrounded)
        {
            inAirLastFrame = false;
            OnLanding.Invoke();
            Debug.Log(name + " Landed");
        }
        else if (!IsGrounded)
        {
            inAirLastFrame = true;
        }
    }

    /// <summary>
    /// Main update method for <see cref="CharacterMotionController"/>.
    /// </summary>
    public override void UpdatePosition()
    {
        StickToGround();
        ApplyGravity();

        SetCombinedMotionVector();
        UpdatePos();

        ApplyDrag();
        ResetMoveVector();
        ResetImpulse();
    }
    /// <summary>
    /// Apply a force that decays over time.
    /// </summary>
    /// <param name="impulse"></param>
    public void DoImpulse(Vector2 impulse)
    {
        ContMotionVector = impulse;
        impulseLastFrame = true;
    }
    public void SetGravityEnabled(bool enabled = true)
    {
        gravityEnabled = enabled;
    }

    protected override void UpdatePos()
    {
        rb.velocity = combinedMotionVector;
    }

    /// <summary>
    /// Combine movement vector with continuous motion applied by gravity
    /// or impulese to create a fintal motion vector.
    /// </summary>
    protected virtual void SetCombinedMotionVector()
    {
        combinedMotionVector = moveVector * XSpeed + ContMotionVector;
    }
    /// <summary>
    /// Method that sticks player to ground if they are close by, allowing smooth ramp descending.
    /// Does not operate if there was an impulse last frame.
    /// </summary>
    protected virtual void StickToGround()
    {
        if (!gravityEnabled) return;
        if (impulseLastFrame) return;

        // Check if close enough to stick to ground
        var stickyRay =
            Physics2D.Raycast(
                new Vector2(
                    col.bounds.center.x,
                    col.bounds.center.y - col.bounds.extents.y
                    ),
                Vector2.down,
                groundStick,
                GroundMask);

        // Stick player to ground if close enough
        if ((bool)stickyRay)
        {
            transform.position =
                new Vector3(
                    transform.position.x,
                    transform.position.y - stickyRay.distance,
                    transform.position.z
                    );
        }
    }

    /// <summary>
    /// Applies gravity effect to continuous motion along the Y axis
    /// </summary>
    protected virtual void ApplyGravity()
    {
        if (!gravityEnabled) return;

        if (IsGrounded)
        {
            if (ContMotionVector.y < 0f) ContMotionVector.y = -0.5f;
        }
        else if (ContMotionVector.y > -GravityMax)
        {
            ContMotionVector.y -= Gravity;
        }
    }
    /// <summary>
    /// Applies drag effects to continuous motion along the X axis
    /// </summary>
    protected virtual void ApplyDrag()
    {
        if (ContMotionVector.x <= -zeroThreshold || ContMotionVector.x >= zeroThreshold)
        {
            ContMotionVector.x -= Math.Sign(ContMotionVector.x) * DragX;
        }
        else
        {
            ContMotionVector.x = 0;
        }
    }

    protected virtual void ResetImpulse()
    {
        impulseLastFrame = false;
    }

}
