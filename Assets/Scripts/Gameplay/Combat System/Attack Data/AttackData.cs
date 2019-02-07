using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/AttackData")]
public class AttackData : ScriptableObject
{
    public int Startup = 12;
    public int Active = 4;
    public int Recovery = 20;

    public float MotionOnAttack = 5f;
    public int Damage = 1;
    public int Strength = 0;
    public float KnockBack = 8f;
    public float KnockUp = 3f;
    public int HitStun = 32;
    public int HitStop = 4;
    public int Sign = 1;
}