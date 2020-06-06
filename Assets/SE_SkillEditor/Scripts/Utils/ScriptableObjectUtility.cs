#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T>(string title, string defaultName, Action<T> setDataAction) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if(setDataAction != null) setDataAction(asset);

        string path = EditorUtility.SaveFilePanelInProject(
        title,
        defaultName + ".asset",
        "asset", "");

        AssetDatabase.CreateAsset(asset, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public static void CloneAsset<T>(string title, string defaultName, T template) where T : ScriptableObject
    {
        T asset = UnityEngine.Object.Instantiate(template) as T;

        string path = EditorUtility.SaveFilePanelInProject(
        title,
        defaultName + ".asset",
        "asset", "");

        AssetDatabase.CreateAsset(asset, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif