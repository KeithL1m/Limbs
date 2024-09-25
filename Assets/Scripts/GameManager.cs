using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static OptionsScreen;

public class GameManager : Manager
{
    [SerializeField] private int _winsNeeded;

    private GameLoader _loader = null;
    private ConfigurationManager _configManager = null;
    private MapManager _mapManager = null;
    private PlayerManager _playerManager = null;
    private ObjectPoolManager _objManager;
    private SceneFade _transition;

    public List<GameObject> spawnPoints = new List<GameObject>();

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private PauseManager _pauseManager;
    private UIManager _uiManager;
    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } }
    private int _playerCount;
    private int _deadPlayers;
    private Player _winningPlayer;

    public bool IsOnline = false;

    public bool startScreen { get; set; } = true;
    public bool VictoryScreen { get; private set; } = false;
    public bool EarlyEnd { get; set; } = false;

    [SerializeField] private EmptyDestructibleObject _empyObj;

    public frameLimits limits;
    private float _changeSceneDuration;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        limits = frameLimits.fps60;
        Application.targetFrameRate = (int)limits;
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        _mapManager = ServiceLocator.Get<MapManager>();
        _playerManager = ServiceLocator.Get<PlayerManager>();
        _objManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    public void SetUp(UIManager uiManager, PauseManager pauseManager)
    {
        GameObject go = Instantiate(_empyObj.gameObject);
        go.name = "DESTROY";
        ServiceLocator.Register<EmptyDestructibleObject>(go.GetComponent<EmptyDestructibleObject>());

        _pauseManager = pauseManager;
        _uiManager = uiManager;
        _transition = _uiManager.GetFade();
        _changeSceneDuration = _transition.GetFadeDuration();

        _playerCount = _configManager.GetPlayerNum();
        _playerConfigs = _configManager.GetPlayerConfigs();
        _configManager.InLoadout = false;

        if (IsOnline)
        {
            var configOnlineManager = ServiceLocator.Get<ConfigurationManagerOnline>();

            _playerCount = configOnlineManager.GetPlayerNum();
            _playerConfigs = configOnlineManager.GetPlayerConfigs();

            SetUpOnline(uiManager, pauseManager);
            return;
        }

        for (int i = 0; i < _playerCount; i++)
        {
            var playerSpawner = _playerConfigs[i].PlayerConfigObject.GetComponent<SpawnPlayer>();
            var playerComp = playerSpawner.SpawnPlayerFirst(_playerConfigs[i]);
            _players.Add(playerComp);

            var playerObj = playerSpawner.Player;
            _playerManager.AddPlayerObject(playerObj);
            Debug.Log($"Adding Player {playerObj.name} to PlayerManager");
        }
    }

    public void SetUpOnline(UIManager uiManager, PauseManager pauseManager)
    {
        for (int i = 0; i < _playerCount; i++)
        {
            //Grabs script and checks if is the host
            var playerSpawner = _playerConfigs[i].PlayerConfigObject.GetComponent<SpawnPlayerOnline>();
            if (!playerSpawner.HasPrivilege())
            {
                return;
            }

            NetworkObject networkObject = _playerConfigs[i].PlayerConfigObject.GetComponent<NetworkObject>();

            ulong clientID = networkObject.OwnerClientId;
            GameObject playerObj = playerSpawner.SpawnPlayerFirst(clientID);
            ulong networkPlayerID = playerObj.GetComponent<NetworkObject>().NetworkObjectId;

            //Sends it to the server so it sends it to all clients and they setUp the characters
            //in their own computer (sprites and stuff)
            playerSpawner.SetCharacterServerRpc(networkPlayerID, i);

            //Does the regular stuff a non online set up would do but only in the host
            Player playerComp = playerObj.GetComponent<Player>();
            _players.Add(playerComp);

            playerObj = playerSpawner.Player;
            _playerManager.AddPlayerObject(playerObj);
            Debug.Log($"Adding Player {playerObj.name} to PlayerManager");
        }
    }

    public void StartGame()
    {
        _uiManager.SetPlayerCount(_playerCount);
        _uiManager.SetUpHealthUI(_players);
        _uiManager.SetPlayerHealthFace(_playerConfigs);

        _uiManager.SetUpLeaderBoard();
        _uiManager.UpdateLeaderBoard();

        _playerManager.ClearLimbs();
        EarlyEnd = false;
        startScreen = false;

        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition()
    {
        _transition.StartTransition();

        yield return new WaitForSeconds(_changeSceneDuration);

        ResetRound();
        _mapManager.LoadMap();
    }

    override public void OnStart()
    {
        GameObject go = Instantiate(_empyObj.gameObject);
        go.name = "DESTROY";
        ServiceLocator.Register<EmptyDestructibleObject>(go.GetComponent<EmptyDestructibleObject>());

        if (!startScreen)
        {
            _pauseManager.SetCamera(Camera.main);
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Debug.Log("Found Spawn Point");
            spawnPoints.Add(gameObjects[i]);
        }

        _playerManager.SetSpawnPoints(spawnPoints);
        _playerManager.OnStartRound();
    }

    public void CheckGameOver()
    {
        PlayerConfiguration winningConfig = null;
        _winningPlayer = null;
        for (int i = 0; i < _playerCount; i++)
        {
            if (_players[i].GetComponent<PlayerHealth>().IsDead())
            {
                _deadPlayers++;
            }
            else
            {
                winningConfig = _playerConfigs[i];
                _winningPlayer = _players[i];
            }
        }

        if (_deadPlayers == _playerCount - 1)
        {
            if (!isGameOver) // Check if game over is not already triggered
            {
                isGameOver = true;
                _winningPlayer.AddScore();
                StartCoroutine(_uiManager.ShowGameOverScreen(winningConfig));
            }
        }
        else
        {
            _deadPlayers = 0;
        }
    }

    public void EndRound()
    {
        _deadPlayers = 0;
        spawnPoints.Clear();

        if (_playerManager.PlayerHasWon(_winsNeeded))
        {
            EnterVictoryScreen();
        }

        if (ServiceLocator.Get<CameraManager>() != null)
        {
            ServiceLocator.Get<CameraManager>().Unregister();
        }

        StartCoroutine(SceneTransition());
    }

    public void VictoryScreenSelect(GameObject button)
    {
        _pauseManager.VictoryScreen(button);
    }

    private void EnterVictoryScreen()
    {
        VictoryScreen = true;

        _playerManager.SortPlayers();
    }

    public void ResetRound()
    {
        ServiceLocator.Get<LimbManager>().ClearList();
        _playerManager.ClearLimbs();
        _playerManager.ResetGroundCheck();
        _uiManager.UpdateLeaderBoard();
        _uiManager.UpdatePlayerWins();
        isGameOver = false;
        _objManager.DeactivateObjects();
        ServiceLocator.Unregister<EmptyDestructibleObject>();
    }

    public void EndGame()
    {
        _deadPlayers = 0;
        spawnPoints.Clear();
        VictoryScreen = false;
        startScreen = true;

        ServiceLocator.Get<AudioManager>().StopMusic();
        for (int i = _playerCount - 1; i >= 0; i--)
        {
            Destroy(_playerConfigs[i].PlayerConfigObject);
            Destroy(_players[i].gameObject);
        }

        ServiceLocator.Unregister<EmptyDestructibleObject>();

        _playerConfigs.Clear();
        _players.Clear();

        _playerManager.ClearLists();

        _configManager.ResetConfigs();

        Destroy(_uiManager.gameObject);

        _playerCount = 0;

        spawnPoints.Clear();

        ServiceLocator.Get<LimbManager>().ClearList();

        SceneManager.LoadScene(3);
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public Player GetWinningPlayer()
    {
        return _winningPlayer;
    }
}
