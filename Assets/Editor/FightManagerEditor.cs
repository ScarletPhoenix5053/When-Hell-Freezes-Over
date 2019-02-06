using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(FightManager))]
public class FightManagerEditor : Editor
{
    Transform InstanceContainer;
    private int StageInEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var fm = target as FightManager;
        
        // Update Transforms in children
        if (fm.InstanceParent != InstanceContainer)
        {
            InstanceContainer = fm.InstanceParent;
            fm.UpdateInstanceParents();
        }

        // Change stage to match currentstage int
        fm.CurrentStage = Convert.ToInt32(EditorGUILayout.Slider(fm.CurrentStage, 0f, fm.Stages.Length));

        if (StageInEditor != fm.CurrentStage)
        {
            if (StageInEditor != 0) fm.Stages[StageInEditor-1].Unload();
            if (fm.CurrentStage != 0) fm.Stages[fm.CurrentStage-1].Load();

            StageInEditor = fm.CurrentStage;
        }
    }
}