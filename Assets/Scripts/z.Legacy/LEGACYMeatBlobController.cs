using UnityEngine;
using System;

public class LEGACYMeatBlobController : EnemyController
{
    public float ChaseRange = 10f;

    protected override void DecideAction()
    {
        FacePlayer();

        if (distToPlayer < ChaseRange)
        {
            SetBehaviour(Behaviour.Chasing);
        }
        else
        {
            SetBehaviour(Behaviour.Idle);
        }
    }
    protected override void Act()
    {
        if (CurrentState == State.Ready)
        {
            switch (CurrentBehaviour)
            {
                case Behaviour.Idle:
                    break;

                case Behaviour.Chasing:
                    if (playerToLeft)
                    {
                        mc.MoveVector = Vector2.left;
                    }
                    else
                    {
                        mc.MoveVector = Vector2.right;
                    }
                    break;

                default:
                    throw new NotImplementedException("Meatblob does not support behaviour " + CurrentBehaviour);
            }
        }
    }
}
