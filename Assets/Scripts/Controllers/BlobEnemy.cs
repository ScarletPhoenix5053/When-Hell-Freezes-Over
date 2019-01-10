using UnityEngine;
using System;

[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(MotionController))]
public class BlobEnemy : MonoBehaviour
{
    public float MinDistToPlayer = 1.2f;

    public bool IsGrounded { get { return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.05f, LayerMask.GetMask("Environment")); } }
    
    private AttackManager am;
    private MotionController mc;
    private PlayerController plr;
    private Behaviour currentBehaviour;

    private enum Behaviour
    {
        Idle, Attacking, Chasing
    }

    private float distToPlayer {  get { return Vector2.Distance(transform.position, plr.transform.position); } }

    private void Awake()
    {
        am = GetComponent<AttackManager>();
        mc = GetComponent<MotionController>();
        plr = FindObjectOfType<PlayerController>();
    }
    private void FixedUpdate()
    {
        // Get Info
        // Decide Action
        // Enact Action

        DecideAction();
        Act();
        Debug.Log(distToPlayer);
    }
    private void LateUpdate()
    {
        mc.ApplyMovement();
        mc.ResetVelocity();
    }
    
    private void DecideAction()
    {
        // if player doesn't exist
        if (plr == null)
        {
            SetBehaviour(Behaviour.Idle);
            return;
        }


        // If player is close enough
        if (distToPlayer < MinDistToPlayer)
        {
            // Attack
            SetBehaviour(Behaviour.Attacking);
        }
        // Else
        else
        {
            // Move closer
            SetBehaviour(Behaviour.Chasing);
        }        
    }
    private void Act()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idle:
                break;

            case Behaviour.Attacking:
                break;

            case Behaviour.Chasing:
                if (plr.transform.position.x < transform.position.x)
                {
                    
                }
                break;

            default:
                break;
        }
    }
    private void SetBehaviour(Behaviour newState)
    {
        currentBehaviour = newState;
    }
}