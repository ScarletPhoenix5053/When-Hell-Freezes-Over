using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MotionController : MonoBehaviour
{
    //public BaseMotionVars Motion = new BaseMotionVars();
    public float Speed = 10f;
    /// <summary>
    /// Used to move player along X axis. Is reset with each call of <see cref="UpdateMotion"/>.
    /// </summary>
    public float InputMotion { get; set; }
    #region Protected Vars
    protected float XDrag = 0.02f;
    protected float YDrag = 0.02f;
    protected const float zeroThreshold = 0.005f;
    protected const int deltaMultiplicationFactor = 50;
    protected bool impulseLastFrame = false;
    protected Rigidbody2D rb;
    protected Vector2 velocity;
    #endregion
    /*
    // update to be struct: will need an accompanying propertydrawer sctipt
    [Serializable]
    public class BaseMotionVars
    {
        public float Speed = 5f;
        public float JumpHeight = 10f;
        public float Acceleration = 0.2f;
        [Range(0, 1)]
        public float DragX = 0.05f;
        public float DragY = 0.00f;
        public float DragZ = 0.05f;
    }*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// currentley the same as SetVelocity(V2). update later!
    /// </summary>
    /// <param name="impulse"></param>
    public void Impulse(Vector2 impulse)
    {
        if (velocity.x < impulse.x) velocity.x = impulse.x;
        if (velocity.y < impulse.y) velocity.y = impulse.y;
        impulseLastFrame = true;
        Debug.Log("read"+velocity.y);
    }
    /// <summary>
    /// Main update method for <see cref="MotionController"/>.
    /// </summary>
    public void UpdateMotion()
    {

        // X AXIS
        InputMotion =Mathf.Clamp(InputMotion, -1, 1);
        
        if (InputMotion < -zeroThreshold || InputMotion > zeroThreshold)
        {
            // Left/Right
            velocity.x = InputMotion * Speed * Time.deltaTime * deltaMultiplicationFactor;
        }
        else
        {
            // Drag
            velocity.x = 0;
        }

        InputMotion = 0;

        ApplyMovement();
        // IMPULSE
        if (impulseLastFrame)
        {
            impulseLastFrame = false;
            velocity = Vector2.zero;
        }
    }    
    /// <summary>
    /// Immediatley set velocities to this force
    /// </summary>
    /// <remarks>
    /// Ignores MaxSpeed
    /// </remarks>
    /// <param name="newVelocity"></param>
    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }
    public void Stop()
    {
        velocity = Vector2.zero;
    }

    /// <summary>
    /// Move the charater
    /// </summary>
    protected void ApplyMovement()
    {
        rb.velocity = new Vector2(velocity.x , velocity.y + rb.velocity.y);
    }
    /// <summary>
    /// Apply drag to the character's X axis exclusivley.
    /// </summary>
    protected void ApplyDrag()
    {
        if (XDrag != 0)
        {
            if (velocity.x != 0)
            {
                if ((velocity.x < zeroThreshold && velocity.x > 0) ||
                    (velocity.x > -zeroThreshold && velocity.x < 0))
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.x -= (XDrag*velocity.x) * Math.Sign(velocity.x);
                }
            }
        }
        
        
    }
}
