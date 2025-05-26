using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeWithColorAttribute))]
public class RangeWithColorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        RangeWithColorAttribute rangeAttribute = (RangeWithColorAttribute)attribute;
        
        // Check if the value is above the warning threshold
        if (property.floatValue > rangeAttribute.warningThreshold)
        {
            // Change the background color to the specified color
            EditorGUI.DrawRect(position, rangeAttribute.color);
        }

        // Draw the property field with a range slider
        EditorGUI.Slider(position, property, rangeAttribute.min, rangeAttribute.max, label);

        if (EditorGUI.EndChangeCheck())
        {
            property.floatValue = Mathf.Clamp(property.floatValue, rangeAttribute.min, rangeAttribute.max);
        }


    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
