using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/ProjectileAttackData")]
public class ProjectileAttackData : AttackData
{
    public float Speed;
    public Vector2 StartDirection;
}