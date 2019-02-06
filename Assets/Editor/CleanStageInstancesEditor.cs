using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CleanStageInstances))]
public class CleanStageInstancesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var csi = target as CleanStageInstances;

        if (GUILayout.Button("Clean all"))
        {
            csi.ClearAll();
        }
    }
}