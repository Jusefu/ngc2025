using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColoredHeaderAttribute))]
public class ColoredHeaderDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        ColoredHeaderAttribute headerAttribute = (ColoredHeaderAttribute)attribute;

        // Store the original GUI color
        Color originalColor = GUI.color;

        // Create the header rectangle
        Rect headerRect = new Rect(position.x, position.y, position.width, headerAttribute.height);

        // Draw the colored background
        EditorGUI.DrawRect(headerRect, headerAttribute.color);

        // Setup text style
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        // Draw the text
        EditorGUI.LabelField(headerRect, headerAttribute.header, style);

        // Restore original GUI color
        GUI.color = originalColor;
    }
}