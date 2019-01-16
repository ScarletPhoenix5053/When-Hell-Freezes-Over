using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMotionController))]
public class PlayerMotionControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Variable heigh jump is hardcoded to \"W\". Please update input system to resolve this!", MessageType.Warning);
    }
}