using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    #region DONE

#if !SerializedProperty_and_serializedObject_Basics

    // References to serialized properties
    SerializedProperty healthProp;
    SerializedProperty speedProp;
    SerializedProperty levelProp;

    void OnEnable()
    {
        // Get references when the inspector is created
        healthProp = serializedObject.FindProperty("health");
        speedProp = serializedObject.FindProperty("speed");
        levelProp = serializedObject.FindProperty("level");
    }

    public override void OnInspectorGUI()
    {
        // Always start with this
        serializedObject.Update();

        // Display health with a custom color if it's low
        EditorGUILayout.PropertyField(healthProp);
        if (healthProp.intValue < 50)
        {
            GUI.color = Color.red;
            EditorGUILayout.HelpBox("Health is low!", MessageType.Warning);
            GUI.color = Color.white;
        }

        // Display speed
        EditorGUILayout.PropertyField(speedProp);

        // Display level with a label indicating tier
        EditorGUILayout.PropertyField(levelProp);
        EditorGUILayout.LabelField("Player Tier: " + GetTierFromLevel(levelProp.intValue));

        // Always end with this
        serializedObject.ApplyModifiedProperties();
    }

    private string GetTierFromLevel(int level)
    {
        if (level < 5) return "Bronze";
        if (level < 15) return "Silver";
        if (level < 30) return "Gold";
        return "Platinum";
    }
#else
    public override void OnInspectorGUI()
    {
        // The simplest implementation just draws the default inspector:
        DrawDefaultInspector();

        // But we can add our own controls too!
        if (GUILayout.Button("Reset Health"))
        {
            PlayerStats playerStats = (PlayerStats)target;
            playerStats.health = 100;

            // Very important: mark the object as dirty!
            EditorUtility.SetDirty(target);
        }
    }
#endif

    #endregion
}