using UnityEngine;
using UnityEditor;

public static class CustomMenuItems
{
    // This adds a new item to the GameObject menu
    [MenuItem("GameObject/Reset Transforms", false, 0)]
    static void ResetSelectedTransforms()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Reset Transforms");
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }

    // This adds a custom menu to the top-level menu bar
    [MenuItem("Tools/Level Design/Create Platform Set")]
    public static void CreatePlatformSet()
    {
        GameObject parent = new GameObject("Platform_Set");

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(10f, 0.5f, 10f);
        floor.transform.position = new Vector3(0, -0.25f, 0);
        floor.transform.SetParent(parent.transform);


        // Register for undo operation
        Undo.RegisterCreatedObjectUndo(parent, "Create Platform Set");
    }



    // The actual command
    [MenuItem("Edit/Level Design/Toggle Grid Snap")]
    static void ToggleGridSnap()
    {
        EditorPrefs.SetBool("UseGridSnap",
                          !EditorPrefs.GetBool("UseGridSnap", false));
    }

    // The validation method
    [MenuItem("Edit/Level Design/Toggle Grid Snap", true)]
    static bool ValidateToggleGridSnap()
    {
        // Return true to enable the menu item
        // Additionally, return a checked state (true = checked)
        Menu.SetChecked("Edit/Level Design/Toggle Grid Snap",
                       EditorPrefs.GetBool("UseGridSnap", false));
        return true;
    }
}