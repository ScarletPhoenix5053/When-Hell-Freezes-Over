using UnityEngine;
using UnityEditor;
using Sierra.Combat2D;

[CustomEditor(typeof(Hurtbox))]
public class HurtboxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hurtbox = target as Hurtbox;

        if (hurtbox.Collider == null)
        {
            Collider2D col = null;
            if (col = hurtbox.GetComponent<Collider2D>())
            {
                hurtbox.Collider = col;
            }
            EditorGUILayout.HelpBox("This component needs a collider 2D attatched to " +
                "the same game object to work!",
                MessageType.Error);
        }
        if (hurtbox.hp == null)
        {
            Health hp = null;
            if (hp = hurtbox.GetComponent<Health>())
            {
                hurtbox.hp = hp;
            }
            EditorGUILayout.HelpBox("This component needs to refer to a health script for " +
                "attack system to work!",
                MessageType.Error);
        }
    }
}