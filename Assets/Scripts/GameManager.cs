using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    [SerializeField] private int _winsNeeded;

    private GameLoader _loader = null;
    private ConfigurationManager _configManager = null;
    private MapManager _mapManager = null;
    private PlayerManager _playerManager = null;
    private ObjectPoolManager _objManager;

    public List<GameObject> spawnPoints = new List<GameObject>();

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private PauseManager _pauseManager;
    private UIManager _uiManager;
    private bool isGameOver = false;

    private int _playerCount;
    private int _deadPlayers;

    public bool startScreen { get; set; } = true;
    public bool VictoryScreen { get; private set; } = false;
    public bool EarlyEnd { get; set; } = false;

    [SerializeField] private EmptyDestructibleObject _empyObj;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
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
        _mapManager.fade = _uiManager.GetFade();

        _playerCount = _configManager.GetPlayerNum();
        _playerConfigs = _configManager.GetPlayerConfigs();
        _configManager.InLoadout = false;

        for (int i = 0; i < _playerCount; i++)
        {
            var playerSpawner = _playerConfigs[i].Input.GetComponent<SpawnPlayer>();
            var playerComp = playerSpawner.SpawnPlayerFirst(_playerConfigs[i]);
            _players.Add(playerComp);

            var playerObj = playerSpawner.Player;
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

        ClearLimbs();
        startScreen = false;
        _mapManager.ChangeScene();
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

        for (int i = 0; i < _playerCount; i++)
        {
            Debug.Log("player is being spawned");
            _players[i].GetComponent<PlayerHealth>().ResetHealth();
            SpawnPlayer(i);
        }
    }

    public void CheckGameOver()
    {
        PlayerConfiguration winningConfig = null;
        Player winningPlayer = null;
        for (int i = 0; i < _playerCount; i++)
        {
            if (_players[i].GetComponent<PlayerHealth>().IsDead())
            {
                _deadPlayers++;
            }
            else
            {
                winningConfig = _playerConfigs[i];
                winningPlayer = _players[i];
            }
        }

        if (_deadPlayers == _playerCount - 1)
        {
            if (!isGameOver) // Check if game over is not already triggered
            {
                isGameOver = true;
                winningPlayer.AddScore();
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
        for (int j = 0; j < _players.Count; j++)
        {
            if (_players[j].GetScore() == _winsNeeded)
            {
                EnterVictoryScreen();
            }
        }

        if (ServiceLocator.Get<CameraManager>() != null)
        {
            ServiceLocator.Get<CameraManager>().Unregister();
        }
        _mapManager.ChangeScene();
    }

    public void VictoryScreenSelect(GameObject button)
    {
        _pauseManager.VictoryScreen(button);
    }

    private void SpawnPlayer(int playerNum)
    {
        _players[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _players[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        _players[playerNum].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        _players[playerNum].GetComponent<PlayerHealth>().isDead = false;
        _players[playerNum].transform.position = spawnPoints[playerNum].transform.position;
    }

    public void ClearLimbs()
    {
        for (int i = 0; i < _playerCount; i++)
        {
            _players[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }

    private void ResetGroundCheck()
    {
        for (int i = 0; i < _playerCount; i++)
        {
            _players[i].GroundCheckTransform.localPosition = new Vector3(0, -0.715f, 0);
        }
    }

    private void EnterVictoryScreen()
    {
        VictoryScreen = true;

        _players.Sort((emp2, emp1) => emp1.GetScore().CompareTo(emp2.GetScore()));
    }

    public void ResetRound()
    {
        ServiceLocator.Get<LimbManager>().ClearList();
        ClearLimbs();
        ResetGroundCheck();
        _uiManager.UpdateLeaderBoard();
        _uiManager.UpdatePlayerWins();
        isGameOver = false;
        _objManager.DeactivateObjects();
        ServiceLocator.Unregister<EmptyDestructibleObject>();
    }

    public void EndGame()
    {
        VictoryScreen = false;
        startScreen = true;
        for (int i = 0; i < _playerCount; i++)
        {
            Destroy(_playerConfigs[i].Input.gameObject);
            Destroy(_players[i].gameObject);
        }

        ServiceLocator.Unregister<EmptyDestructibleObject>();

        _playerConfigs.Clear();
        _players.Clear();

        _playerManager.ClearList();

        _configManager.ResetConfigs();

        Destroy(_uiManager.gameObject);

        _playerCount = 0;

        spawnPoints.Clear();
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }
}
