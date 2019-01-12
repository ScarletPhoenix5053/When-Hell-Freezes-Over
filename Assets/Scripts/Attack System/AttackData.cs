using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/AttackData")]
public class AttackData : ScriptableObject
{
    public int Damage = 1;
    public int Startup = 12;
    public int Active = 4;
    public int Recovery = 20;
    public int HitStun = 32;

    public float KnockBack = 8f;
    public float KnockUp = 3f;
}