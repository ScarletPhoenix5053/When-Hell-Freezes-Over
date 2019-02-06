using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackManager : EnemyAttackManager
{
    public override void Attack()
    {
        DoRangedAttack(0);
    }
}
