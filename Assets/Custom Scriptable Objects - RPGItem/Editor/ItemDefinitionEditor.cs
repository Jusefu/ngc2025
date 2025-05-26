using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using System;
using UnityEngine;

[CustomEditor(typeof(ItemDefinition))]
public class ItemDefinitionEditor : Editor
{
    SerializedProperty itemNameProp;
    SerializedProperty descriptionProp;
    SerializedProperty iconProp;
    SerializedProperty rarityProp;
    SerializedProperty itemTypeProp;
    SerializedProperty goldValueProp;
    SerializedProperty requiredLevelProp;
    SerializedProperty isStackableProp;
    SerializedProperty maxStackSizeProp;
    SerializedProperty damageAmountProp;
    SerializedProperty attackSpeedProp;
    SerializedProperty critChanceProp;
    SerializedProperty armorValueProp;
    SerializedProperty movementPenaltyProp;
    SerializedProperty healthRestoredProp;
    SerializedProperty manaRestoredProp;
    SerializedProperty durationProp;

    // Foldout states
    private bool showBaseStats = true;
    private bool showTypeProperties = true;

    void OnEnable()
    {
        // Cache serialized properties
        itemNameProp = serializedObject.FindProperty("itemName");
        descriptionProp = serializedObject.FindProperty("description");
        iconProp = serializedObject.FindProperty("icon");
        rarityProp = serializedObject.FindProperty("rarity");
        itemTypeProp = serializedObject.FindProperty("itemType");
        goldValueProp = serializedObject.FindProperty("goldValue");
        requiredLevelProp = serializedObject.FindProperty("requiredLevel");
        isStackableProp = serializedObject.FindProperty("isStackable");
        maxStackSizeProp = serializedObject.FindProperty("maxStackSize");
        damageAmountProp = serializedObject.FindProperty("damageAmount");
        attackSpeedProp = serializedObject.FindProperty("attackSpeed");
        critChanceProp = serializedObject.FindProperty("critChance");
        armorValueProp = serializedObject.FindProperty("armorValue");
        movementPenaltyProp = serializedObject.FindProperty("movementPenalty");
        healthRestoredProp = serializedObject.FindProperty("healthRestored");
        manaRestoredProp = serializedObject.FindProperty("manaRestored");
        durationProp = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ItemDefinition item = (ItemDefinition)target;

        // Create a nice header with item preview
        DrawHeaderWithPreview(item);

        // Basic Properties Section
        showBaseStats = EditorGUILayout.Foldout(showBaseStats,
                                              "Basic Properties",
                                              true,
                                              EditorStyles.foldoutHeader);
        if (showBaseStats)
        {
            // Draw basic properties like name, description, icon, etc.
            DrawBasicProperties();

            // Draw rarity with colors
            DrawRarityField();

            // Value field with gold icon
            DrawValueField();

            // Conditional max stack size based on stackable toggle
            EditorGUILayout.PropertyField(isStackableProp);
            if (isStackableProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(maxStackSizeProp);
                EditorGUI.indentLevel--;
            }
        }

        // Type-specific Properties Section - changes based on item type!
        ItemDefinition.ItemType itemType = (ItemDefinition.ItemType)itemTypeProp.enumValueIndex;
        
        if (showTypeProperties)
        {
            switch (itemType)
            {
                case ItemDefinition.ItemType.Weapon:
                    showTypeProperties = EditorGUILayout.Foldout(showTypeProperties,
                                                          $"{itemType} Properties",
                                                          true,
                                                          EditorStyles.foldoutHeader);
                    DrawWeaponProperties();
                    break;

                case ItemDefinition.ItemType.Armor:
                    showTypeProperties = EditorGUILayout.Foldout(showTypeProperties,
                                                   $"{itemType} Properties",
                                                   true,
                                                   EditorStyles.foldoutHeader);
                    DrawArmorProperties();
                    break;

                case ItemDefinition.ItemType.Potion:
                    showTypeProperties = EditorGUILayout.Foldout(showTypeProperties,
                                                   $"{itemType} Properties",
                                                   true,
                                                   EditorStyles.foldoutHeader);
                    DrawPotionProperties();
                    break;
            }
        }

        // Add preview and validation buttons
        DrawPreviewAndValidationSection(item);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawPreviewAndValidationSection(ItemDefinition item)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview"))
        {
            // Open a preview window or perform preview action
            Debug.Log("Previewing item: " + item.itemName);
        }
        if (GUILayout.Button("Validate"))
        {
            // Perform validation checks
            //ValidateItem(item);
        }
        EditorGUILayout.EndHorizontal();

    }

    private void DrawPotionProperties()
    {
        
        EditorGUILayout.PropertyField(healthRestoredProp);
        EditorGUILayout.PropertyField(manaRestoredProp);
        EditorGUILayout.PropertyField(durationProp);
    }

    private void DrawArmorProperties()
    {
        //EditorGUILayout.PropertyField(armorValueProp);
        EditorGUILayout.IntSlider(armorValueProp, 0, 100, "Armor");
        DrawStatBar("Armor stats", armorValueProp.intValue, 100, new Color(0.01f, 0.2f,0.5f));

        //EditorGUILayout.PropertyField(movementPenaltyProp);
        EditorGUILayout.Slider(movementPenaltyProp, 0f, 10f, "Movement Penalty");
        DrawStatBar("Movement stats", movementPenaltyProp.floatValue, 10, Color.red);

    }

    private void DrawBasicProperties()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Item Type");
        string[] enums = Enum.GetNames(typeof(ItemDefinition.ItemType));


        GUIStyle style = new GUIStyle(EditorStyles.miniButton);
        style.border = new RectOffset(2, 2, 2, 2);
        for (int i = 0; i < enums.Length; i++)
        {
            GUIStyle buttonStyle = itemTypeProp.intValue == i ? style : EditorStyles.miniButton;
            
            if (GUILayout.Button(enums[i], buttonStyle))
            {
                itemTypeProp.enumValueIndex = i;
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(itemNameProp);
        EditorGUILayout.PropertyField(descriptionProp);
        EditorGUILayout.PropertyField(goldValueProp);
        EditorGUILayout.PropertyField(requiredLevelProp);
    }

    // Helper methods for specialized drawing
    private void DrawHeaderWithPreview(ItemDefinition item) {
        // Create a header area with icon preview
        Color rarityColor = GetRarityColor(item.rarity);
        Rect headerRect = EditorGUILayout.GetControlRect(false, 60);
        
        EditorGUI.DrawRect(headerRect, rarityColor * 0.5f);

        // Draw icon preview
        if (item.icon != null)
        {
            Rect iconRect = new Rect(headerRect.x + 10,
                                   headerRect.y + 5, 50, 50);
            GUI.DrawTexture(iconRect, item.icon.texture);
        }

        // Draw item name in large font
        GUIStyle nameStyle = new GUIStyle(EditorStyles.largeLabel);
        nameStyle.fontSize = 16;
        nameStyle.fontStyle = FontStyle.Bold;

        Rect nameRect = new Rect(headerRect.x + 70,
                               headerRect.y + 10,
                               headerRect.width - 80, 20);
        EditorGUI.LabelField(nameRect, item.itemName, nameStyle);

        
        Rect rarityRect = new Rect(headerRect.x + 70,
                                 headerRect.y + 30,
                                 headerRect.width - 80, 20);
        EditorGUI.LabelField(rarityRect, item.rarity.ToString() + " " + item.itemType.ToString());


    }

    private Color GetRarityColor(ItemDefinition.ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemDefinition.ItemRarity.Common:
                return Color.white;
            case ItemDefinition.ItemRarity.Uncommon:
                return Color.green;
            case ItemDefinition.ItemRarity.Rare:
                return Color.blue;
            case ItemDefinition.ItemRarity.Epic:
                return new Color(0.5f, 0, 0.5f); // Purple
            case ItemDefinition.ItemRarity.Legendary:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    private void DrawRarityField()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Rarity");
        string[] enums = Enum.GetNames(typeof(ItemDefinition.ItemRarity));
        for (int i = 0; i < enums.Length; i++)
        {
            // Draw rarity with colored text
            
            if (GUILayout.Button(enums[i], EditorStyles.miniButton))
            {
                rarityProp.enumValueIndex = i;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawValueField() { /* ... */ }
    private void DrawWeaponProperties() {
        // Damage with colored bar
        EditorGUILayout.PropertyField(damageAmountProp);
        DrawStatBar("Damage", damageAmountProp.intValue, 100, Color.red);

        // Attack speed with slider and text label
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Attack Speed");
        attackSpeedProp.floatValue = EditorGUILayout.Slider(
            attackSpeedProp.floatValue, 0.5f, 3.0f);
        EditorGUILayout.EndHorizontal();

        string speedText = attackSpeedProp.floatValue < 1.0f ? "Slow" :
                         attackSpeedProp.floatValue < 1.5f ? "Average" :
                         attackSpeedProp.floatValue < 2.0f ? "Fast" :
                         "Very Fast";
        EditorGUILayout.LabelField("Speed Rating: " + speedText,
                                 EditorStyles.miniLabel);

        // Crit chance as percentage
        float critPercent = critChanceProp.floatValue * 100f;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Critical Chance");
        float newCritPercent = EditorGUILayout.Slider(critPercent, 0f, 100f);
        EditorGUILayout.LabelField(newCritPercent.ToString("0") + "%",
                                 GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        critChanceProp.floatValue = newCritPercent / 100f;

    }
    private void DrawStatBar(string label, float value, float maxValue,
                        Color barColor)
    {
        // Draw a colored bar representing a stat value
        Rect rect = EditorGUILayout.GetControlRect(false, 18);
        rect.x += 10;
        rect.width -= 20;

        float percentage = Mathf.Clamp01(value / maxValue);

        // Draw background
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));

        // Draw filled portion
        Rect fillRect = new Rect(rect.x, rect.y,
                              rect.width * percentage, rect.height);
        EditorGUI.DrawRect(fillRect, barColor);

        // Draw label
        string text = $"{label}: {value}/{maxValue}";
        GUIStyle centeredStyle = new GUIStyle(EditorStyles.miniBoldLabel);
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        centeredStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(rect, text, centeredStyle);
    }
}