using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardTracker))]
public class KeyboardTrackerEditor : Editor
{
    KeyboardTracker keyTracker;
    public override void OnInspectorGUI()
    {
        keyTracker = target as KeyboardTracker;
        DrawAxes();
        DrawButtons();
    }

    public void DrawAxes()
    {
        EditorGUILayout.LabelField("Axes", EditorStyles.boldLabel);
        if (keyTracker.AxisKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No axes defined in input manager!", MessageType.Info); ;
        }
        else
        {
            SerializedProperty property = serializedObject.FindProperty("AxisKeys");
            for (int i = 0; i < keyTracker.AxisKeys.Length; i++)
            {
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent("Axis " + i));
            }
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
    public void DrawButtons()
    {
        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
        if (keyTracker.ButtonKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No buttons defined in input manager!", MessageType.Info); ;
        }
        else
        {
            for (int i = 0; i < keyTracker.ButtonKeys.Length; i++)
            {
                keyTracker.ButtonKeys[i] = (KeyCode)EditorGUILayout.EnumPopup("Button " + i, keyTracker.ButtonKeys[i]);
            }
        }
    }
}