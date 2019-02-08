using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{    
    /// <summary>
    /// Speed of this character.
    /// </summary>
    public float XSpeed = 10f;
    /// <summary>
    /// Optional Y speed. Mainly for use with zero-g projectiles & entities.
    /// </summary>
    public float YSpeed = 0f;
    /// <summary>
    /// Stops movement when <see cref="true"/>.
    /// </summary>    
    public bool Disbled { get; protected set; }
    /// <summary>
    /// A normalized vector used to determine which direction the character should be moving.
    /// Use <see cref="XSpeed"/> to affect movement speed.
    /// </summary>
    public Vector2 MoveVector
    {
        get { return moveVector; }
        set { moveVector = value.normalized; }
    }

    protected Rigidbody2D rb;
    [ReadOnly][SerializeField]
    protected Vector2 moveVector;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void UpdatePosition()
    {
        UpdatePos();
        ResetMoveVector();
    }

    /// <summary>
    /// Apply transformation to rigidbody
    /// </summary>
    protected virtual void UpdatePos()
    {
        rb.velocity = new Vector2(MoveVector.x * XSpeed, MoveVector.y * YSpeed);
    }
    /// <summary>
    /// Reset move vector for next update cycle
    /// </summary>
    protected void ResetMoveVector()
    {
        moveVector = Vector2.zero;
    }
}
