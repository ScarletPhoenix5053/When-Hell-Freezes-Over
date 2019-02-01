using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAttackManager))]
public class PlayerAttackManagerEditor : Editor
{
    MeleeWeaponItem meleeItem;
    RangedWeaponItem rangedItem;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var atkManager = (PlayerAttackManager)target;

        // display help boxes if weapon refrences are null
        if (atkManager.MeleeWeapon == null)
        {
            EditorGUILayout.HelpBox("Please assign a melee weapon!", MessageType.Error);
        }
        if (atkManager.RangedWeapon == null)
        {
            EditorGUILayout.HelpBox("No ranged weapon assigned. Player behaviour will be adjusted accordingly.", MessageType.Warning);
        }
    }
}