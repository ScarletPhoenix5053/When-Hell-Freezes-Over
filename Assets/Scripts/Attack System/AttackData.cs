using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    public int Damage = 300;
    public int Startup = 12;
    public int Active = 4;
    public int Recovery = 20;
    public int BlockStun = 14;
    public int HitStun = 32;

    public float KnockBack = 8f;
    public float KnockUp = 3f;
}
