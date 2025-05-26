using UnityEditor;
using UnityEngine;

public class SimpleWindow : EditorWindow
{

    [MenuItem("Window/Simple Window")]
    public static void ShowWindow()
    {
        // Create a new window
        SimpleWindow window = GetWindow<SimpleWindow>("Simple Window");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("This is a simple window", EditorStyles.boldLabel);
        if(GUILayout.Button("Click Me"))
        {
            Debug.Log("Button clicked!");
        }

    }
}
