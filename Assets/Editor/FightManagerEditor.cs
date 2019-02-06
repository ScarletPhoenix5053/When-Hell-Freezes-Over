using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FightManager))]
public class FightManagerEditor : Editor
{
    Transform InstanceContainer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var fm = target as FightManager;
        
        if (fm.InstanceParent != InstanceContainer)
        {
            InstanceContainer = fm.InstanceParent;
            fm.UpdateInstanceParents();
        }
    }
}