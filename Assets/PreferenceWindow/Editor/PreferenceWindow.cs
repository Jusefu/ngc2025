using UnityEditor;
using UnityEngine;

public class PreferenceWindow : EditorWindow
{
    // Settings we want to persist
    private string projectName = "New Project";
    private bool autoSave = true;
    private Color themeColor = Color.blue;

    // Keys for EditorPrefs
    private const string PROJECT_NAME_KEY = "MyWindow_ProjectName";
    private const string AUTO_SAVE_KEY = "MyWindow_AutoSave";
    private const string THEME_COLOR_KEY = "MyWindow_ThemeColor";

    [MenuItem("Tools/Preferences Window")]
    public static void ShowWindow()
    {
        GetWindow<PreferenceWindow>("Preferences");
    }

    void OnEnable()
    {
        // Load saved preferences
        projectName = EditorPrefs.GetString(PROJECT_NAME_KEY, "New Project");
        autoSave = EditorPrefs.GetBool(AUTO_SAVE_KEY, true);

        // For complex types like Color, we need to store individual components
        float r = EditorPrefs.GetFloat(THEME_COLOR_KEY + "_r", 0);
        float g = EditorPrefs.GetFloat(THEME_COLOR_KEY + "_g", 0);
        float b = EditorPrefs.GetFloat(THEME_COLOR_KEY + "_b", 1);
        themeColor = new Color(r, g, b);
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        projectName = EditorGUILayout.TextField("Project Name", projectName);
        autoSave = EditorGUILayout.Toggle("Auto Save", autoSave);
        themeColor = EditorGUILayout.ColorField("Theme Color", themeColor);

        if (EditorGUI.EndChangeCheck())
        {
            // Save preferences when they change
            EditorPrefs.SetString(PROJECT_NAME_KEY, projectName);
            EditorPrefs.SetBool(AUTO_SAVE_KEY, autoSave);

            EditorPrefs.SetFloat(THEME_COLOR_KEY + "_r", themeColor.r);
            EditorPrefs.SetFloat(THEME_COLOR_KEY + "_g", themeColor.g);
            EditorPrefs.SetFloat(THEME_COLOR_KEY + "_b", themeColor.b);
        }
    }
}