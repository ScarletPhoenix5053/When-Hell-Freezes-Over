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

        // If either weapon item changes
        if (meleeItem != atkManager.MeleeWeapon ||
            rangedItem != atkManager.RangedWeapon)
        {
            meleeItem = atkManager.MeleeWeapon;
            rangedItem = atkManager.RangedWeapon;

            // update attackData array
            atkManager.AssignWeaponAttackData();


        }

        // display help boxes if weapon refrences are null
        if (meleeItem == null)
        {
            EditorGUILayout.HelpBox("Please assign a melee weapon!", MessageType.Error);
        }
        if (rangedItem == null)
        {
            EditorGUILayout.HelpBox("No ranged weapon assigned. Player behaviour will be adjusted accordingly.", MessageType.Warning);
        }
    }
}