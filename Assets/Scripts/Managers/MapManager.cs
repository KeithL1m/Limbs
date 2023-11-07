using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Manager
{
    private GameLoader _loader = null;
    private GameManager _gm = null;

    public int _mapCount;

    private static System.Random rnd = new System.Random();

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        _gm = ServiceLocator.Get<GameManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadMap()
    {
        int mapNum = rnd.Next(4, _mapCount + 3);
        SceneManager.LoadScene(mapNum);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_gm.startScreen)
            return;
        Debug.Log("new scene loaded");
        _gm.OnStart();
    }
}
