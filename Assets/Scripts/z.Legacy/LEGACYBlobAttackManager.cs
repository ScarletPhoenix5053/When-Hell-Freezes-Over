using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class LEGACYBlobAttackManager : EnemyAttackManager, IHitboxResponder
{
    public override void Attack()
    {
        DoAttack(0);
    }
}
