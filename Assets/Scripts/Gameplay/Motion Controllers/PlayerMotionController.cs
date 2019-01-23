using UnityEngine;
using System.Collections;

public class PlayerMotionController : CharacterMotionController
{
    public float FallMultiplier = 1.5f;
    public float LowJumpMultiplier = 1.5f;

    protected override void ApplyGravity()
    {
        if (IsGrounded)
        {
            if (ContMotionVector.y < 0f) ContMotionVector.y = -0.5f;
        }
        // If not falling faster than max falling speed
        else if (ContMotionVector.y > -GravityMax)
        {
            // ...and is actually falling, not rising
            if (ContMotionVector.y < 0)
                ContMotionVector.y -= Gravity * FallMultiplier;
            // ...otherwise must be rising
            else if
                (ContMotionVector.y > 0 && !InputManager.JumpHeld())
                ContMotionVector.y -= Gravity * LowJumpMultiplier;
            else
                ContMotionVector.y -= Gravity;
        }
    }
}
