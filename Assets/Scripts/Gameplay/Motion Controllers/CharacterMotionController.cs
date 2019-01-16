using System;
using UnityEngine;

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
            return Physics2D.Raycast(
                new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y),
                Vector2.down,
                groundBuffer,
                LayerMask.GetMask("Environment"));
        }
    }
    #endregion
    #region Protected Vars
    protected BaseController chr;
    protected Collider2D col;

    protected const float groundBuffer = 0.1f;
    protected const float zeroThreshold = 0.05f;
    protected const int deltaMultiplicationFactor = 50;

    protected bool impulseLastFrame = false;
    protected Vector2 contMotionVector;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        chr = GetComponent<BaseController>();
        col = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Main update method for <see cref="CharacterMotionController"/>.
    /// </summary>
    public override void UpdatePosition()
    {
        // apply gravity to cont motion vector
        if (col != null)
        {
            if (IsGrounded)
            {
                if (contMotionVector.y < 0f) contMotionVector.y = -0.5f;
            }
            else if (contMotionVector.y < GravityMax)
            {
                contMotionVector.y -= Gravity;
            }
        }


        // create combined motion vector
        var combinedMotion = moveVector * XSpeed + contMotionVector;
        
        // update position
        rb.velocity = combinedMotion;

        // reset movevector for next cycle
        moveVector = Vector2.zero;

        // apply drag to cont motion for next cycle
        if (contMotionVector.x <= -zeroThreshold || contMotionVector.x >= zeroThreshold)
        {
            contMotionVector.x -= Math.Sign(contMotionVector.x) * DragX;
        }
        else
        {
            contMotionVector.x = 0;
        }
    }
    /// <summary>
    /// Apply a force that decays over time.
    /// </summary>
    /// <param name="impulse"></param>
    public void DoImpulse(Vector2 impulse)
    {
        contMotionVector = impulse;
        impulseLastFrame = true;
    }
}
