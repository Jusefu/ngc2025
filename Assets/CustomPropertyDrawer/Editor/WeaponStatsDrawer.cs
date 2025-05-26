using UnityEditor;
using UnityEngine;
using static WeaponStats;

[CustomPropertyDrawer(typeof(WeaponStats))]
public class WeaponStatsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        
        SerializedProperty damageProp = property.FindPropertyRelative("damage");
        SerializedProperty critChanceProp = property.FindPropertyRelative("critChance");
        SerializedProperty attackSpeedProp = property.FindPropertyRelative("attackSpeed");
        SerializedProperty elementProp = property.FindPropertyRelative("element");


        float damage = damageProp.floatValue;
        float critChance = critChanceProp.floatValue;
        float attackSpeed = attackSpeedProp.floatValue;
        WeaponStats.ElementType elementType = (WeaponStats.ElementType)elementProp.enumValueIndex;

        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width,
                    EditorGUIUtility.singleLineHeight), property.isExpanded,
            label + " " + elementType.ToString() +  " Dmg: " + damage + " Crit: " + critChance + " AS " + attackSpeed,
            true);

        EditorGUI.DrawRect(position, GetElementBackgroundColor(elementType));

        if (property.isExpanded)
        {

            // Get indented position to account for indentation level
            Rect indentedPosition = EditorGUI.IndentedRect(position);
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            // Create properly indented rect for each property
            Rect damageRect = new Rect(indentedPosition.x, position.y + lineHeight, indentedPosition.width, lineHeight);
            EditorGUI.PropertyField(damageRect, damageProp);

            Rect critRect = new Rect(indentedPosition.x, damageRect.y + lineHeight + spacing, indentedPosition.width, lineHeight);
            EditorGUI.PropertyField(critRect, critChanceProp);

            Rect speedRect = new Rect(indentedPosition.x, critRect.y + lineHeight + spacing, indentedPosition.width, lineHeight);
            EditorGUI.PropertyField(speedRect, attackSpeedProp);

            Rect elementRect = new Rect(indentedPosition.x, speedRect.y + lineHeight + spacing, indentedPosition.width, lineHeight);
            EditorGUI.PropertyField(elementRect, elementProp);

            EditorGUI.indentLevel--;
        }

            EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Base height is one line
        float height = EditorGUIUtility.singleLineHeight;

        // If expanded, add height for each property plus spacing
        if (property.isExpanded)
        {
            // Add 4 properties (damage, critChance, attackSpeed, element)
            height += EditorGUIUtility.singleLineHeight * 4;

            // Add spacing between lines if needed
            height += EditorGUIUtility.standardVerticalSpacing * 3;
        }

        return height;
    }

    // Helper method to get element background color
    private Color GetElementBackgroundColor(WeaponStats.ElementType elementType)
    {
        switch (elementType)
        {
            case WeaponStats.ElementType.Fire:
                return new Color(0.7f, 0.3f, 0.2f, 0.2f);
            case WeaponStats.ElementType.Ice:
                return new Color(0.2f, 0.5f, 0.7f, 0.2f);
            case WeaponStats.ElementType.Lightning:
                return new Color(0.7f, 0.7f, 0.2f, 0.2f);
            case WeaponStats.ElementType.Poison:
                return new Color(0.4f, 0.7f, 0.3f, 0.2f);
            default:
                return new Color(0.3f, 0.3f, 0.3f, 0.1f);
        }
    }

}
