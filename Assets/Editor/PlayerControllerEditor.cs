using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Currentley using a hardcoded input sytem. Please re-design inputs!", MessageType.Warning);
    }
}