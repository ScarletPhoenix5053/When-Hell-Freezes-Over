using UnityEngine;
using System;


[RequireComponent(typeof(MotionController))]
public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get { return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.05f, LayerMask.GetMask("Environment")); } }
    public float JumpHeight = 12f;

    private PlayerAttackManager am;
    private MotionController mc;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0;    

    private void Awake()
    {
        am = GetComponent<PlayerAttackManager>();
        mc = GetComponent<MotionController>();
    }
    private void FixedUpdate()
    {
        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        // Light attack button
        if (data.buttons[0] && 
            !am.Attacking)
        {
            am.LightAttack();
        }

        // Jump
        if (data.axes[0] > 0.5 && IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.Impulse(new Vector2(0, JumpHeight));
        }
        // Walk
        if (data.axes[1] !=  0)
        {
            mc.InputMotion = data.axes[1];
        }
    }   
}