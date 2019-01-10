﻿using UnityEngine;
using System;

[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(MotionController))]
public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get { return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.05f, LayerMask.GetMask("Environment")); } }
    
    private AttackManager am;
    private MotionController mc;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0; 

    private enum AttackState
    {

    }
    

    private void Awake()
    {
        am = GetComponent<AttackManager>();
        mc = GetComponent<MotionController>();
    }
    private void FixedUpdate()
    {
        mc.ApplyMovement();
        mc.ResetVelocity();

        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        // Light attack button
        if (data.buttons[0])
        {
            if (am.LightAttack())
            {
            }
            else
            {
                Debug.LogWarning("Cannot chain from " + am.AtkState);            
            }
        }
        // Launcher attack button
        if (data.buttons[1])
        {
            if (am.LauncherAttack())
            {
            }
        }

        // Jump
        if (data.axes[0] > 0.5 && IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.UpdateVelocity(new Vector3(mc.XVel, mc.Motion.JumpHeight * Time.fixedDeltaTime * 50, 0));
        }
        // Walk
        if (data.axes[1] !=  0)
        {            
            mc.UpdateVelocity(
                new Vector3(
                    data.axes[1] * mc.Motion.Speed * Time.fixedDeltaTime * 50,
                    mc.YVel, 0));
        }
    }

   
}