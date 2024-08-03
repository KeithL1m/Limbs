using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Limbs/Debug Settings")]
public class DebugSettings : ScriptableObject
{
    [SerializeField] private string _sceneToLoad;
    public string NextScene => _sceneToLoad;
    [SerializeField] private List<string> _allScenes = new List<string>();

    public void PopulateSceneList(List<string> sceneNames)
    {
        _allScenes.Clear();
        _allScenes.AddRange(sceneNames);
    }
}
