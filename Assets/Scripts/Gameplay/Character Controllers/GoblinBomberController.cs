using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBomberController : EnemyController
{
    // NORMAL STUN: on any melee, causes a held bomb to be dropped
    // SUPERSTUN: on critical ranged, causes a held bomb to be dropped

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Act()
    {
        // ATTACK:
        // launch bombs at player on regular timer @ a fixed angle
        throw new System.NotImplementedException();
    }
    protected override void DecideAction()
    {
        // If player in range, face player & start attacking
        // otherwise: idle
        throw new System.NotImplementedException();
    }
}
