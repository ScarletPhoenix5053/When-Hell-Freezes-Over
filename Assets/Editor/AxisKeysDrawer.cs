using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AxisKeys))]
public class AxisKeysDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // do not indent
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;


        // initial label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // establish rects  
        Rect posLabel = new Rect(position.x, position.y, 15, position.y);
        Rect posField = new Rect(position.x + 12, position.y, 50, position.y);
        Rect negLabel = new Rect(position.x + 65, position.y, 15, position.y);
        Rect negField = new Rect(position.x + 75, position.y, 50, position.y);

        // set labels
        GUIContent posGUI = new GUIContent("+");
        GUIContent negGUI = new GUIContent("-");

        // draw fields
        EditorGUI.LabelField(posLabel, posGUI);
        EditorGUI.PropertyField(posField, property.FindPropertyRelative("Positive"), GUIContent.none);
        EditorGUI.LabelField(negLabel, negGUI);
        EditorGUI.PropertyField(negField, property.FindPropertyRelative("Negative"), GUIContent.none);

        // reset indent
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}