using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class PlayerAttackManager : AttackManager, IHitboxResponder
{
    public bool LightAttack()
    {
        var success = false;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = DoAttack(0);
        StartCoroutine(activeCoroutine);

        return success;
    }
}
