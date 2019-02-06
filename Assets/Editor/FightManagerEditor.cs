using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(FightManager))]
public class FightManagerEditor : Editor
{
    Transform InstanceContainer;
    private int StageInEditor;
    private List<List<GameObject>> StageObjectsInEditor = new List<List<GameObject>>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var fm = target as FightManager;

        // Make Sure StageObjectsInEditor is assigned
        if (StageObjectsInEditor.Count == 0)
        {
            if (fm.StageCount == 0)
            {

            }
            else if (fm.CurrentStage != 0)
            {
                for (int i = 0; i < fm.Stages[fm.CurrentStage - 1].Instances.Length; i++)
                {
                    StageObjectsInEditor.Add(fm.Stages[fm.CurrentStage - 1].Instances[i].SpawnedObjects);
                }
            }
        }
        
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

            // Keep StageObjectsInEditor up to date
            if (fm.CurrentStage != 0)
            {
                for (int i = 0; i < fm.Stages[fm.CurrentStage - 1].Instances.Length; i++)
                {
                    StageObjectsInEditor = new List<List<GameObject>>();
                    StageObjectsInEditor.Add(fm.Stages[fm.CurrentStage - 1].Instances[i].SpawnedObjects);
                }
            }

            StageInEditor = fm.CurrentStage;
        }

        // Match inspector position to object position in scene
        if (fm.CurrentStage != 0)
        {
            for (int i = 0; i < fm.Stages[StageInEditor - 1].Instances.Length; i++)
            {
                for (int o = 0; o < fm.Stages[StageInEditor - 1].Instances[i].SpawnedObjects.Count; o++)
                {
                    if (fm.Stages[StageInEditor - 1].Instances[i].SpawnedObjects[o].transform.position 
                        != (Vector3)fm.Stages[StageInEditor - 1].Instances[i].SpawnLocations[o])
                    {
                        fm.Stages[StageInEditor - 1].Instances[i].SpawnedObjects[o].transform.position
                            = (Vector3)fm.Stages[StageInEditor - 1].Instances[i].SpawnLocations[o];
                    }
                }
            }
        }
    }
}