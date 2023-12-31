using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Manager
{
    private GameLoader _loader = null;
    private GameManager _gm = null;

    [SerializeField] private int _mapCount;
    [SerializeField] private int _loadingMaps;
    [SerializeField] private int _victoryScreen;

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
#if LIMBS_DEBUG
        var debugSceneName = ServiceLocator.Get<DebugSettings>().NextScene;
        if (string.IsNullOrWhiteSpace(debugSceneName) == false)
        {
            SceneManager.LoadScene(debugSceneName);
            return;
        }
#endif

        if (_gm.VictoryScreen)
        {
            SceneManager.LoadScene(_victoryScreen);
        }
        else if (_gm.EarlyEnd)
        {
            return;
        }
        else
        {
            int mapNum = rnd.Next(_loadingMaps, _mapCount);
            SceneManager.LoadScene(mapNum);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_gm.startScreen)
            return;
        Debug.Log("New scene loaded");
        _gm.OnStart();
    }
}
