using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugSettings))]
public class DebugSettingsEditor : Editor
{
    private static DebugSettings _target;
    private List<string> _sceneNames = new();
    private void OnEnable()
    {
        _target = target as DebugSettings;
        _sceneNames = new List<string>();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Refresh Scene List"))
        {
            RefreshSceneNames();
            _target.PopulateSceneList(_sceneNames);
        }
    }

    private void RefreshSceneNames()
    {
        _sceneNames.Clear();
        var allAssets = AssetDatabase.GetAllAssetPaths();
        for (int i = 0; i < allAssets.Length; ++i)
        {
            if ( allAssets[i].StartsWith("Assets/Scenes/_Playable Maps/") && allAssets[i].EndsWith(".unity"))
            {
                _sceneNames.Add(allAssets[i]);
            }
        }
        
        Debug.Log($"Found {_sceneNames.Count} Scenes in the AssetDatabase");
    }
}
