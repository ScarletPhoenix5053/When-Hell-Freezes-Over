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
            if (contMotionVector.y < 0f) contMotionVector.y = -0.5f;
        }
        // If not falling faster than max falling speed
        else if (contMotionVector.y > -GravityMax)
        {
            // ...and is actually falling, not rising
            if (contMotionVector.y < 0)
                contMotionVector.y -= Gravity * FallMultiplier;
            // ...otherwise must be rising
            else if
                (contMotionVector.y > 0 && !Input.GetKey(KeyCode.W))
                contMotionVector.y -= Gravity * LowJumpMultiplier;
            else
                contMotionVector.y -= Gravity;
        }
    }
}
