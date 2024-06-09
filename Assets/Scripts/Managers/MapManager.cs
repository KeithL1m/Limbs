using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Manager
{
    private GameLoader _loader = null;
    private GameManager _gm = null;

    public SceneFade fade;
    
    private int _mapCount;
    [SerializeField] private int _loadingMaps;
    private int _victoryScreen;
    int currentMap;

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

        _mapCount = SceneManager.sceneCountInBuildSettings - 2;
        _victoryScreen = SceneManager.sceneCountInBuildSettings - 1;
    }

    public void ChangeScene()
    {
        fade.FadeOut = true;
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
            //Check if map is repeated
            int mapNum = Random.Range(_loadingMaps + 1, _mapCount);
            while (mapNum == currentMap)
            {
                Debug.Log("MAP WAS REPEATED");
                mapNum = Random.Range(_loadingMaps + 1, _mapCount);
            }
            currentMap = mapNum;
            SceneManager.LoadScene(mapNum);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_gm.startScreen || _gm.EarlyEnd)
            return;
        Debug.Log("New scene loaded");
        _gm.OnStart();
    }
}
