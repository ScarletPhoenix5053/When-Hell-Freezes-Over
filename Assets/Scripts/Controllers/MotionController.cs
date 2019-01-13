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

    protected const float groundBuffer = 0.05f;
    protected const float zeroThreshold = 0.005f;
    protected const int deltaMultiplicationFactor = 50;

    protected bool impulseLastFrame = false;
    protected Vector2 moveVector;
    protected Vector2 frameMotion;
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

        // X AXIS
        if (!InputOverride)
        {
            if (MoveVector.x < -zeroThreshold || MoveVector.x > zeroThreshold)
            {
                // Left/Right
                frameMotion.x = MoveVector.x * Speed * Time.deltaTime * deltaMultiplicationFactor;
            }
            else
            {
                // Drag
                frameMotion.x = 0;
            }

            // REMEMBER TO RESET MOVEVECTOR
            //MoveVector.x = 0;
        }

        ApplyMovement();

        // IMPULSE
        if (impulseLastFrame)
        {
            impulseLastFrame = false;
            frameMotion = Vector2.zero;
        }
    }
    /// <summary>
    /// Apply a force that decays over time.
    /// </summary>
    /// <param name="impulse"></param>
    public void DoImpulse(Vector2 impulse)
    {
        if (frameMotion.x < impulse.x) frameMotion.x = impulse.x;
        if (frameMotion.y < impulse.y) frameMotion.y = impulse.y;
        impulseLastFrame = true;
    }

    public void EnableInputOverride() => InputOverride = true;
    public void DisableInputOverride() => InputOverride = false;

    /// <summary>
    /// Move the charater
    /// </summary>
    protected void ApplyMovement()
    {
        if (name == "Blob" && InputOverride) Debug.Log("v"+frameMotion.x);
        if (!InputOverride)
        {
            rb.velocity = new Vector2(frameMotion.x, frameMotion.y + rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x + frameMotion.x, rb.velocity.y + frameMotion.y);
            Debug.Log("r" + rb.velocity.x);
        }    
    }
}
