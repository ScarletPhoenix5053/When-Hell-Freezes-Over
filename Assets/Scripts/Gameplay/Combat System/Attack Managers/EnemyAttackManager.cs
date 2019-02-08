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
    public virtual void RangedAttack()
    {
        DoRangedAttack(1);
    }
    public virtual void RangedAttack(Vector2 dir)
    {
        DoRangedAttack(1, dir);
    }
    public virtual void DoRangedAttack(int attackIndex, Vector2 dir)
    {
        CheckAttackIndexIsInRange(attackIndex);
        currentAttackIndex = attackIndex;

        // Create Projectile
        CreateProjectile();

        // Configure projectile
        ConfigureProjectile(dir);

        // Start attack routine
        StartRangedAttackRoutine();
    }
    protected void ConfigureProjectile(Vector2 dir)
    {
        var projControl = projectiles[0].GetComponent<ProjectileController>();
        var projMc = projControl.GetComponent<MotionController>();

        projControl.SetAttackData(Attacks[1]);
        projControl.SetHitboxResponder(projControl);

        dir = dir.normalized;
        projMc.XSpeed = dir.x * 50 * Math.Sign(transform.localScale.x);
        projMc.YSpeed = dir.y * 50;
    }
}
