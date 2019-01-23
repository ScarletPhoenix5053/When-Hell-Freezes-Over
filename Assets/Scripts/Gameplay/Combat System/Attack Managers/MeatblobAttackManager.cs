using UnityEngine;
using System.Collections;

public class MeatblobAttackManager : EnemyAttackManager
{
    public override void DoAttack(int attackIndex)
    {
        currentAttackIndex = attackIndex;
        Hitbox.SetResponder(this);
        Hitbox.SetActive();
    }
}
