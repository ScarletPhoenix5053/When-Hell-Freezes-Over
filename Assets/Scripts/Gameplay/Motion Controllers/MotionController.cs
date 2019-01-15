using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MotionController : MonoBehaviour
{
    #region Public Vars
    /// <summary>
    /// Speed of this character.
    /// </summary>
    public float Speed = 10f;
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
    /// Stops movement when <see cref="true"/>.
    /// </summary>
    public bool InputOverride { get; protected set; }
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
    /// <summary>
    /// A normalized vector used to determine which direction the character should be moving.
    /// Use <see cref="Speed"/> to affect movement speed.
    /// </summary>
    public Vector2 MoveVector
    {
        get { return moveVector; }
        set { moveVector = value.normalized; }
    }
    #endregion
    #region Protected Vars
    protected Rigidbody2D rb;
    protected Collider2D col;

    protected const float groundBuffer = 0.1f;
    protected const float zeroThreshold = 0.05f;
    protected const int deltaMultiplicationFactor = 50;

    protected bool impulseLastFrame = false;
    protected Vector2 moveVector;
    protected Vector2 contMotionVector;
    #endregion

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Main update method for <see cref="MotionController"/>.
    /// </summary>
    public void UpdatePosition()
    {
        // apply gravity to cont motion vector
        if (IsGrounded)
        {
            if (contMotionVector.y < 0f) contMotionVector.y = -0.5f;
        }
        else if (contMotionVector.y < GravityMax)
        {
            contMotionVector.y -= Gravity;
        }


        // create combined motion vector
        var combinedMotion = moveVector * Speed + contMotionVector;

        // deebug
        if (name == "Blob")
        {
            Debug.Log("MV: " + MoveVector);
            Debug.Log("ContMV: " + contMotionVector);
            Debug.Log("CombMV: " + combinedMotion);
        }

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

    public void EnableInputOverride() => InputOverride = true;
    public void DisableInputOverride() => InputOverride = false;
}
