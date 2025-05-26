using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MixedGUI : EditorWindow
{
    [MenuItem("Window/Mixed GUI")]
    public static void ShowWindow()
    {
        // Create a new window
        MixedGUI window = (MixedGUI)GetWindow(typeof(MixedGUI));
        window.titleContent = new GUIContent("Mixed GUI");
        window.Show();
    }

    void OnGUI()
    {
        // Use layout for most of the UI
        EditorGUILayout.LabelField("My Tools", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Tool 1")) { Debug.Log("Tool 1 pressed..."); }
        if (GUILayout.Button("Tool 2")) { Debug.Log("Tool 2 pressed..."); }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);

        // Switch to fixed positioning for a custom visualization
        Rect graphRect = GUILayoutUtility.GetRect(200, 100);
        DrawCustomGraph(graphRect);
    }

    void DrawCustomGraph(Rect rect)
    {
        // Draw a simple graph using GUI
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));

        // Draw graph lines
        Handles.color = Color.white;
        

        // Draw data points
        for (int i = 0; i < 5; i++)
        {
            float x = rect.x + (rect.width / 4) * i;
            float y = rect.y + rect.height * 0.5f + Mathf.Sin(i * 0.5f) * (rect.height * 0.4f);


            float x2 = rect.x + (rect.width / 4) * (i + 1);
            float y2 = rect.y + rect.height * 0.5f + Mathf.Sin((i + 1) * 0.5f) * (rect.height * 0.4f);

            Handles.DrawSolidDisc(new Vector3(x, y), Vector3.forward, 5);
            Handles.DrawLine(new Vector3(x, y),new Vector3(x2, y2));
        }
    }
}
