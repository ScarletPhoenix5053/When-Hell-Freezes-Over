using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/AttackData")]
public class AttackData : ScriptableObject
{
    public DamageData DamageData;

    public int Startup = 12;
    public int Active = 4;
    public int Recovery = 20;
    public float ImpulseStrength = 5f;

    public int Damage { get { return DamageData.Damage; } }
    public int HitStun { get { return DamageData.HitStun; } }
    public int HitStop { get { return DamageData.HitStop; } }
    public int Sign { get { return DamageData.Sign; } set { DamageData.Sign = value; } }
    public float KnockBack { get { return DamageData.KnockBack; } }
    public float KnockUp { get { return DamageData.KnockUp; } }
}