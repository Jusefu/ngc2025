using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// Editor Window that demonstrates a long-running task using editor coroutines
public class EditorCoroutineExample : EditorWindow
{
    private bool isProcessRunning = false;
    private float progress = 0f;
    private string statusMessage = "Ready to start";
    private List<string> results = new List<string>();
    private EditorCoroutine runningCoroutine;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Editor Coroutine Example")]
    public static void ShowWindow()
    {
        GetWindow<EditorCoroutineExample>("Coroutine Demo");
    }

    private void OnGUI()
    {
        GUILayout.Label("Editor Coroutine Demo", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        // Only enable the start button if no process is running
        GUI.enabled = !isProcessRunning;
        if (GUILayout.Button("Start Long Process", GUILayout.Height(30)))
        {
            StartLongProcess();
        }
        GUI.enabled = true;

        // Show progress bar when a task is running
        if (isProcessRunning)
        {
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), progress, statusMessage);

            if (GUILayout.Button("Cancel", GUILayout.Height(25)))
            {
                if (runningCoroutine != null)
                {
                    EditorCoroutineUtility.StopCoroutine(runningCoroutine);
                    runningCoroutine = null;
                }

                isProcessRunning = false;
                statusMessage = "Operation canceled.";
            }
        }

        EditorGUILayout.Space(10);

        // Display results in a scrollable area
        GUILayout.Label("Results:", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

        foreach (string result in results)
        {
            EditorGUILayout.HelpBox(result, MessageType.Info);
        }

        EditorGUILayout.EndScrollView();
    }

    private void StartLongProcess()
    {
        isProcessRunning = true;
        progress = 0f;
        statusMessage = "Starting long process...";
        results.Clear();

        // Start the coroutine
        runningCoroutine = EditorCoroutineUtility.StartCoroutine(FindMissingReferencesCoroutine(), this);
    }

    private IEnumerator FindMissingReferencesCoroutine()
    {
        // Get all scenes in the project
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new string[] { "Assets/EditorCoroutine" });
        Debug.Log($"Found {scenePaths.Length} scenes.");
        for (int i = 0; i < scenePaths.Length; i++)
        {
            // Update progress for the UI
            progress = (float)i / scenePaths.Length;
            statusMessage = $"Checking scene {i + 1}/{scenePaths.Length}";

            // Get the actual path and open the scene
            string scenePath = AssetDatabase.GUIDToAssetPath(scenePaths[i]);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            // Find all GameObjects in the scene
            GameObject[] rootObjects = scene.GetRootGameObjects();

            // Check each GameObject for issues
            foreach (GameObject rootObject in rootObjects)
            {
                Component[] components = rootObject.GetComponentsInChildren<Component>(true);

                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        results.Add($"Missing script in: {rootObject.name} of  {scenePath}");
                        continue;
                    }

                    // Check serialized properties for missing references
                    SerializedObject serializedObject = new SerializedObject(component);
                    SerializedProperty iterator = serializedObject.GetIterator();

                    while (iterator.NextVisible(true))
                    {
                        if (iterator.propertyType == SerializedPropertyType.ObjectReference &&
                            iterator.objectReferenceValue == null &&
                            iterator.objectReferenceInstanceIDValue != 0)
                        {
                            results.Add($"Missing reference in {component.GetType().Name}");
                            break;
                        }
                    }
                }

                // Yield occasionally to keep the editor responsive
                if (rootObject.transform.childCount > 10)
                    yield return null;
            }

            // Close the scene and yield to keep the editor responsive
            EditorSceneManager.CloseScene(scene, false);
            yield return null;
        }

        // Update UI when complete
        progress = 1f;
        statusMessage = "Finished searching for missing references!";
        isProcessRunning = false;
    }
}

// Utility class to handle editor coroutines
public static class EditorCoroutineUtility
{
    public static EditorCoroutine StartCoroutine(IEnumerator routine, EditorWindow owner)
    {
        EditorCoroutine coroutine = new EditorCoroutine(routine, owner);
        coroutine.Start();
        return coroutine;
    }

    public static void StopCoroutine(EditorCoroutine coroutine)
    {
        coroutine.Stop();
    }
}

// Class that wraps the coroutine execution
public class EditorCoroutine
{
    private IEnumerator routine;
    private EditorWindow owner;
    private bool stopped = false;

    public EditorCoroutine(IEnumerator routine, EditorWindow owner)
    {
        this.routine = routine;
        this.owner = owner;
    }

    public void Start()
    {
        EditorApplication.update += Update;
    }

    public void Stop()
    {
        stopped = true;
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        if (stopped) return;

        if (!routine.MoveNext())
        {
            Stop();
            return;
        }

        // Force the owner window to repaint to show progress
        if (owner != null)
        {
            owner.Repaint();
        }
    }
}