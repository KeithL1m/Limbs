using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : ASyncLoader
{
    [SerializeField] private GameObject _gameManager = null;
    [SerializeField] private int sceneIndexToLoad = 1;
    private static int _sceneIndex = 1;
    private static GameLoader _instance; // only singleton that should be here.

    [SerializeField] private List<Component> _moduleComponents = new List<Component>();

    public static Transform SystemsParent { get { return _systemsParent; } }
    private static Transform _systemsParent;

    protected override void Awake()
    {
        Debug.Log("GameLoader Booting Up!");

        //Safety Check
        if(_instance != null && _instance != this)
        {
            Debug.Log("A duplicate instance of the GameLoader was found and will be ignored. Only 1 instance is allowed ");
            Destroy(gameObject);
            return;
        }

        // Set reference to this instance
        _instance = this;

        // Make persistent
        DontDestroyOnLoad(gameObject);

        // Scene Index Check
        if(sceneIndexToLoad == 0) 
        {
            _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
        else if (sceneIndexToLoad < 0 || sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Invalid Scene Index {sceneIndexToLoad} ... using default value of {_sceneIndex}");
        }
        else
        {
            _sceneIndex = sceneIndexToLoad;
        }

        // Setup System GameObject
        GameObject systemsGameObject = new GameObject("[Systems]");
        _systemsParent = systemsGameObject.transform;
        DontDestroyOnLoad(systemsGameObject);

        // Queue up loading routines
        Enqueue(InitializeCoreSystems(), 1);
        Enqueue(InitializeModularSystems(), 2);

        // Register the GameLoader
        ServiceLocator.Register<GameLoader>(this);

        // set completion callback
        CallOnComplete(OnComplete);

    }

    private IEnumerator InitializeCoreSystems()
    {
        // Setup Core Systems
        Debug.Log("Loading Core Systems");

        var gm = Instantiate(_gameManager, SystemsParent);
        ServiceLocator.Register<GameManager>(gm.GetComponent<GameManager>());
        ServiceLocator.Register<MapManager>(gm.GetComponent<MapManager>());

        yield return null;
    }

    private IEnumerator InitializeModularSystems()
    {
        // Setup Additional Systems as needed
        Debug.Log("Loading Modular Systems");

        foreach (var comp in _moduleComponents)
        {
            if(comp is IGameModule)
            {
                var module = comp as IGameModule;
                yield return module.LoadModule();
            }
        }

        yield return null;
    }

    private void OnComplete()
    {
        Debug.Log("GameLoader Completed");
        StartCoroutine(LoadInitialScene(_sceneIndex));
    }

    private IEnumerator LoadInitialScene(int index)
    {
        Debug.Log("GameLoader Starting Scene Load");
        yield return SceneManager.LoadSceneAsync(index);
    }
}
