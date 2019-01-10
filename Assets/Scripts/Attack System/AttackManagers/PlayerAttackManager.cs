using Sierra;
using System;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class BlobAttackManager : AttackManager, IHitboxResponder
{
    public bool Attack()
    {
        var success = false;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = DoAttack(0);
        StartCoroutine(activeCoroutine);

        return success;
    }
}
