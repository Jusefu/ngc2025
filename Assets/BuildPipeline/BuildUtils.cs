using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildUtils : MonoBehaviour
{
    [MenuItem("Build/Create Development Build")]
    public static void BuildDevelopment()
    {
        PerformBuild(false);
    }

    [MenuItem("Build/Create Release Build")]
    public static void BuildRelease()
    {
        PerformBuild(true);
    }

    static void PerformBuild(bool isReleaseBuild)
    {
        Debug.Log("Starting automated build process...");

        // 1. Pre-build validation
        if (!ValidateProject())
        {
            Debug.LogError("Pre-build validation failed! Build aborted.");
            return;
        }

        // 2. Update version info
        UpdateVersionInfo(isReleaseBuild);

        // 3. Configure build settings
        ConfigureBuildSettings(isReleaseBuild);

        // 4. Perform the build
        string buildPath = GetBuildPath(isReleaseBuild);
        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = GetScenesToBuild(),
            locationPathName = buildPath,
            target = EditorUserBuildSettings.activeBuildTarget,
            options = GetBuildOptions(isReleaseBuild)
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        // 5. Process build results
        ProcessBuildResult(report);
    }

  
    private static bool ValidateProject()
    {
        // Perform any necessary validation checks here
        // For example, check for missing references, invalid assets, etc.
        foreach (var obj in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj == null)
            {
                Debug.LogError("Found a null GameObject reference!");
                return false;
            }
        }

        Debug.Log("Project validation passed.");
        return true;
    }


    private static void UpdateVersionInfo(bool isReleaseBuild)
    {
        // Update version info in a scriptable object or player settings
        PlayerSettings.bundleVersion = DateTime.Now.ToString("yyyyMMdd.HHmm");
        PlayerSettings.productName = isReleaseBuild ? "MyGame_Release" : "MyGame_Dev";
        Debug.Log("Version info updated.");
    }

    private static void ConfigureBuildSettings(bool isReleaseBuild)
    {

        // Configure build settings based on the build type
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)), isReleaseBuild ? "RELEASE_BUILD" : "DEVELOPMENT_BUILD");
        Debug.Log("Build settings configured.");
    }

    private static string[] GetScenesToBuild()
    {
        // Get all enabled scenes in the build settings
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static string GetBuildPath(bool isReleaseBuild)
    {
        // Define the build path based on the build type
        string buildFolder = isReleaseBuild ? "ReleaseBuilds" : "DevelopmentBuilds";
        return System.IO.Path.Combine(Application.dataPath, "..", buildFolder, PlayerSettings.productName);
    }

    private static BuildOptions GetBuildOptions(bool isReleaseBuild)
    {
        // Set build options based on the build type
        return isReleaseBuild ? BuildOptions.None : BuildOptions.Development | BuildOptions.ConnectWithProfiler;
    }

    private static void ProcessBuildResult(BuildReport report)
    {
        // Process the build result and log any errors or warnings
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded!");
        }
        else
        {
            Debug.LogError($"Build failed with errors: {report.summary.totalErrors}");
        }
    }
}
