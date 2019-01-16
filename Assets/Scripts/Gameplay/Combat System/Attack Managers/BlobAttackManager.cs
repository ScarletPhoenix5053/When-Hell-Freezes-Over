using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class BlobAttackManager : EnemyAttackManager, IHitboxResponder
{
    public override void Attack()
    {
        DoAttack(0);
    }
}
