
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalField))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
                             GUIContent label)
    {
        ConditionalField condAttr = attribute as ConditionalField;

        // Find the conditional property (should be a boolean)
        SerializedProperty conditionProperty =
            property.serializedObject.FindProperty(condAttr.conditionalFieldName);

        // Get the value (accounting for possible inversion)
        bool showField = condAttr.inverted ?
            !conditionProperty.boolValue : conditionProperty.boolValue;

        // If condition is met, draw the field normally
        if (showField)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        // Otherwise, don't draw anything
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalField condAttr = attribute as ConditionalField;
        SerializedProperty conditionProperty =
            property.serializedObject.FindProperty(condAttr.conditionalFieldName);

        // Handle if the conditional property can't be found
        if (conditionProperty == null)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        // Get the value (accounting for possible inversion)
        bool showField = condAttr.inverted ?
            !conditionProperty.boolValue : conditionProperty.boolValue;

        if (showField)
        {
            // Calculate the complete height including all children
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        return 0f;
    }
}