using ProjectGame;
using UnityEditor;
using UnityEngine;
using RangeInt = ProjectGame.RangeInt;

[CustomPropertyDrawer(typeof(RangeInt))]
public class RangeIntDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;


        SerializedProperty min = property.FindPropertyRelative(nameof(RangeInt.min));
        SerializedProperty max = property.FindPropertyRelative(nameof(RangeInt.max));
        
        int[] values = new int[] {min.intValue, max.intValue};
        EditorGUI.indentLevel = indent;
        position.height = 18f;
        EditorGUI.BeginChangeCheck();
        GUIContent[] subLabels = {new GUIContent("min"), new GUIContent("max")};
        EditorGUI.MultiIntField(position, subLabels, values);

        values[0] = Mathf.Min(values[0], values[1]);
        values[1] = Mathf.Max(values[1], values[0]);
        
        if (EditorGUI.EndChangeCheck())
        {
            min.intValue = values[0];
            max.intValue = values[1];
        }
        
        EditorGUI.EndProperty();
    }
}