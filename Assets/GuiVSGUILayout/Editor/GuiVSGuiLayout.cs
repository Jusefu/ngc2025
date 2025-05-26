
using UnityEditor;
using UnityEngine;


public class GuiVSGuiLayout : EditorWindow
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [MenuItem("Window/GUI vs GUILayout")]
    public static void ShowWindow()
    {
        // Create a new window
        GuiVSGuiLayout window = (GuiVSGuiLayout)GetWindow(typeof(GuiVSGuiLayout));
        window.titleContent = new GUIContent("GUI vs GUILayout");
        window.Show();
    }

    void OnGUI()
    {
#if GUI
        // Draw a label at a specific position with specific size
        EditorGUI.LabelField(new Rect(10, 10, 200, 20), "Hello World");

        // Draw a button at a specific position with specific size
        if (GUI.Button(new Rect(10, 40, 100, 30), "Click Me"))
        {
            Debug.Log("Button clicked");
        }
#else
        // Draw a label, letting the system determine position
        EditorGUILayout.LabelField("Hello World");
    
        // Draw a button, letting the system determine position
        if (GUILayout.Button("Click Me"))
        {
            Debug.Log("Button clicked");
        }
#endif
    }
}
