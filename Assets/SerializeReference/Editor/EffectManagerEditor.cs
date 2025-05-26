using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

// Custom editor for the EffectManager
[CustomEditor(typeof(EffectManager))]
public class EffectManagerEditor : Editor
{
    // SerializedProperty for the effects list
    private SerializedProperty effectsProperty;

    // Available effect types for the dropdown
    private List<Type> availableEffectTypes = new List<Type>();
    private string[] effectTypeNames;
    private int selectedEffectType = 0;

    private void OnEnable()
    {
        // Get the serialized effects list
        effectsProperty = serializedObject.FindProperty("statusEffects");

        // Find all types that implement IStatusEffect and are serializable
        FindAvailableEffectTypes();

        // Create array of type names for the dropdown
        effectTypeNames = new string[availableEffectTypes.Count];
        for (int i = 0; i < availableEffectTypes.Count; i++)
        {
            effectTypeNames[i] = availableEffectTypes[i].Name;
        }
    }

    private void FindAvailableEffectTypes()
    {
        availableEffectTypes.Clear();

        // Find all types in all loaded assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                // Check if type implements IStatusEffect, is not an interface, and has [Serializable] attribute
                if (typeof(IStatusEffect).IsAssignableFrom(type) &&
                    !type.IsInterface &&
                    !type.IsAbstract &&
                    Attribute.IsDefined(type, typeof(System.SerializableAttribute)))
                {
                    availableEffectTypes.Add(type);
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Status Effects Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Display the list of current effects
        EditorGUILayout.PropertyField(effectsProperty, true);

        EditorGUILayout.Space(10);

        // Draw a separator line
        Rect rect = EditorGUILayout.GetControlRect(false, 2);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

        EditorGUILayout.Space(10);

        // Add new effect section
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add New Effect:", GUILayout.Width(120));
        selectedEffectType = EditorGUILayout.Popup(selectedEffectType, effectTypeNames);

        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            AddNewEffect();
        }
        EditorGUILayout.EndHorizontal();

        //EditorGUILayout.Space(5);
        //EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button("Add Poison Effect"))
        //{
        //    AddEffect(typeof(PoisonEffect));
        //}

        //if (GUILayout.Button("Add Slow Effect"))
        //{
        //    AddEffect(typeof(SlowEffect));
        //}
        //EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void AddNewEffect()
    {
        if (selectedEffectType >= 0 && selectedEffectType < availableEffectTypes.Count)
        {
            AddEffect(availableEffectTypes[selectedEffectType]);
        }
    }

    private void AddEffect(Type effectType)
    {
        // Get the actual EffectManager component
        EffectManager manager = target as EffectManager;

        // Create a new instance of the selected effect type
        IStatusEffect newEffect = (IStatusEffect)Activator.CreateInstance(effectType);

        // Add it to the list
        manager.statusEffects.Add(newEffect);

        // Mark the object as dirty
        EditorUtility.SetDirty(target);

        // Refresh serialization
        serializedObject.Update();
    }
}