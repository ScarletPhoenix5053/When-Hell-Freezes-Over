using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class EnemyAttackManager : AttackManager, IHitboxResponder
{
    public virtual void Attack()
    {
        DoAttack(0);
    }
}
