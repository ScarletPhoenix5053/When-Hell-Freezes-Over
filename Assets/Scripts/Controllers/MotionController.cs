using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MotionController : MonoBehaviour
{
    public BaseMotionVars Motion = new BaseMotionVars();

    public float XVel = 0f;
    public float YVel = 0f;
    public float ZVel = 0f;

    protected const float zeroThreshold = 0.005f;
    protected Rigidbody2D rb;
    
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
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // update char movement
    /// <summary>
    /// Immediatley set velocities to this force
    /// </summary>
    /// <remarks>
    /// Ignores MaxSpeed
    /// </remarks>
    /// <param name="force"></param>
    public void UpdateVelocity(Vector3 force)
    {
        XVel = force.x;
        YVel = force.y;
        ZVel = force.z;
    }
    /// <summary>
    /// resets velocity on ALL axes!
    /// </summary>
    public void ResetVelocity()
    {
        XVel = 0;
        YVel = 0;
        ZVel = 0;
    }
    /// <summary>
    /// resets velocity on ALL axes!
    /// </summary>
    public void ResetVelocityHor()
    {
        XVel = 0;
        ZVel = 0;
    }
    /// <summary>
    /// Move the charater
    /// </summary>
    public void ApplyMovement()
    {
        rb.velocity = new Vector3(XVel, rb.velocity.y + YVel, ZVel);
    }
    /// <summary>
    /// Apply drag to the character's Z axis exclusivley.
    /// </summary>
    public void ApplyDrag()
    {
        // apply X drag
        if (Motion.DragX != 0)
        {
            if (XVel != 0)
            {
                if ((XVel < zeroThreshold && XVel > 0) ||
                    (XVel > -zeroThreshold && XVel < 0))
                {
                    XVel = 0;
                }
                else
                {
                    XVel -= (Motion.DragX*XVel) * Math.Sign(XVel);
                }
            }
        }
        
        
    }
}
