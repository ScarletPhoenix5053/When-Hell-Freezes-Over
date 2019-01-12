using Sierra;
using System;
using System.Collections;
using Sierra.Combat2D;
using UnityEngine;

public class BlobAttackManager : AttackManager, IHitboxResponder
{
    public bool Attack()
    {
        var success = false;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = IE_DoAttack(0);
        StartCoroutine(activeCoroutine);

        return success;
    }
}
