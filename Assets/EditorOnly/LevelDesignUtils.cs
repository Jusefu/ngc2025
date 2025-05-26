using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using System.IO;

public static class LevelDesignUtils
{
    // Create a utilities submenu
    [MenuItem("Tools/Level Design Utilities/Setup New Level")]
    static void SetupNewLevel()
    {
        // Create a new scene
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);

        // Add standard level components
        GameObject levelRoot = new GameObject("_LevelRoot");
        levelRoot.AddComponent<LevelController>();

        GameObject environment = new GameObject("Environment");
        environment.transform.SetParent(levelRoot.transform);

        GameObject lighting = new GameObject("Lighting");
        lighting.transform.SetParent(levelRoot.transform);
        lighting.AddComponent<Light>().type = LightType.Directional;

        GameObject enemies = new GameObject("Enemies");
        enemies.transform.SetParent(levelRoot.transform);
        enemies.AddComponent<EnemyManager>();

        GameObject playerSpawn = new GameObject("PlayerSpawn");
        playerSpawn.transform.SetParent(levelRoot.transform);

        // Select the root object
        Selection.activeGameObject = levelRoot;

        Debug.Log("New level created with standard hierarchy!");
    }

    [MenuItem("Tools/Level Design Utilities/Create Standard Folders")]
    static void CreateStandardFolders()
    {
        string[] folders = {
            "Assets/EditorOnly/Levels",
            "Assets/EditorOnly/Levels/Prefabs",
            "Assets/EditorOnly/Levels/Materials",
            "Assets/EditorOnly/Levels/Textures",
            "Assets/EditorOnly/Levels/Scripts"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Standard level design folders created!");
    }

    // Add keyboard shortcuts with %=Ctrl, #=Shift, &=Alt
    [MenuItem("Tools/Level Design Utilities/Validate Current Level _F5")]
    static void ValidateCurrentLevel()
    {
        // Find all enemy spawners
        EnemySpawner[] spawners = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        int errors = 0;
        if(spawners.Length == 0)
        {
            Debug.LogError("No enemy spawners found in the scene!");
            errors++;
        }

        // Check for common level design issues
        foreach (EnemySpawner spawner in spawners)
        {
            if (spawner.enemyPrefab == null)
            {
                Debug.LogError($"Spawner {spawner.name} has no enemy prefab assigned!", spawner);
                errors++;
            }
        }

        // Final validation report
        if (errors == 0)
        {
            EditorUtility.DisplayDialog("Level Validation",
                "Level passed all validation checks!", "Great!");
        }
        else
        {
            EditorUtility.DisplayDialog("Level Validation",
                $"Level has {errors} critical issues to fix. Check the console for details.",
                "I'll Fix Them");
        }
    }
}