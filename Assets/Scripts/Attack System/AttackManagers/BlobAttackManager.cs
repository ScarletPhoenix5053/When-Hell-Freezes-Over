using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class BlobAttackManager : AttackManager, IHitboxResponder
{
    public void Attack()
    {
        DoAttack(0);
    }
}
