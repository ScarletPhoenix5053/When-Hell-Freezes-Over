using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/DamageData")]
public class DamageData : ScriptableObject
{
    public int Damage = 1;
    public float KnockBack = 8f;
    public float KnockUp = 3f;
    public int HitStun = 32;
    public int HitStop = 4;
    public int Sign = 1;
}