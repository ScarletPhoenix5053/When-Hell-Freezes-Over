using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackManager))]
public class DemonController : EnemyController
{

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Idle, Throwing
    }

    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour != CurrentBehaviour)
            CurrentBehaviour = newBehaviour;
    }

    protected override void Act()
    {
        throw new System.NotImplementedException();
    }
    protected override void DecideAction()
    {
        throw new System.NotImplementedException();
    }
}
