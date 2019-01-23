using System;
using UnityEngine;
using UnityEditor;
using Sierra;

public class MeatblobController : EnemyController
{
    public float DetectionRange = 12f;
    public float MinimumAttackRange = 2f;
    public float MaximumAttackRange = 5f;
    [Range(0, 1)]
    public float EnterWindupChance = 0.1f;
    [Range(0, 1)]
    public float LeapChance = 0.1f;

    public Behaviour CurrentBehaviour = Behaviour.Idle;
    public enum Behaviour { Idle, Chasing, WindingUp, Leaping, Recovering }

    protected void OnDrawGizmosSelected()
    {
        DrawCircle(DetectionRange, Color.cyan);
        DrawCircle(MaximumAttackRange, Color.green);
        DrawCircle(MinimumAttackRange, Color.red);
    }

    /// <summary>
    /// Change enemy's behaviour state
    /// </summary>
    /// <param name="newBehaviour"></param>
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour == CurrentBehaviour) return;

        //Debug.Log(name + " changed behaviour state from " + currentBehaviour + " to " + newBehaviour);
        CurrentBehaviour = newBehaviour;
    }

    protected override void Act()
    {
        switch (CurrentBehaviour)
        {
            case Behaviour.Chasing:
                FacePlayer();
                var dir = 0;
                if (PlayerToLeft) dir = -1; else dir = 1;
                mc.MoveVector = new Vector2(dir, 0);
                break;

            case Behaviour.WindingUp:
                break;

            case Behaviour.Leaping:
                break;

            case Behaviour.Recovering:
                break;

            default:
                break;
        }
    }
    protected override void DecideAction()
    {
        switch (CurrentBehaviour)
        {
            case Behaviour.Idle:
                if (DistToPlayer < DetectionRange) SetBehaviour(Behaviour.Chasing);
                break;

            case Behaviour.Chasing:
                if ((DistToPlayer <= MinimumAttackRange) || 
                    (DistToPlayer <= MaximumAttackRange && Utility.GetRandomFloat() < EnterWindupChance)
                    )
                    SetBehaviour(Behaviour.WindingUp);
                break;

            case Behaviour.WindingUp:                
                break;

            case Behaviour.Leaping:
                break;

            case Behaviour.Recovering:
                break;

            default:
                break;
        }
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }

}
