using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Manager
{
    private GameLoader _loader = null;
    private ConfigurationManager _configManager = null;
    private MapManager _mapManager = null;
    private PlayerManager _playerManager = null;

    public List<PlayerInput> playerList = new List<PlayerInput>();
    public List<GameObject> spawnPoints = new List<GameObject>();

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private PauseManager _pauseManager;
    private UIManager _uiManager;
    private bool isGameOver = false;

    private int playerCount;
    public int deadPlayers;

    public bool startScreen = true;


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
    }

    public void SetUp(UIManager uiManager, PauseManager pauseManager)
    {
        _pauseManager = pauseManager;
        _uiManager = uiManager;

        playerCount = _configManager.GetPlayerNum();
        _playerConfigs = _configManager.GetPlayerConfigs();

        for (int i = 0; i < playerCount; i++)
        {
            var playerSpawner =_playerConfigs[i].Input.GetComponent<SpawnPlayer>();
            var playerComp = playerSpawner.SpawnPlayerFirst(_playerConfigs[i]);
            _players.Add(playerComp);
            playerList.Add(_playerConfigs[i].Input);

            var playerObj = playerSpawner.Player;
            _playerManager.AddPlayerObject(playerObj);
            Debug.Log($"Adding Player {playerObj.name} to PlayerManager");
        }
    }

    override public void OnStart()
    {
        _pauseManager.SetCamera(Camera.main);

        _uiManager.UpdatePlayerWins(_playerConfigs);

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            spawnPoints.Add(gameObjects[i]);
        }

        for (int i = 0; i < playerCount; i++)
        {
            Debug.Log("player is being spawned");
            _players[i].GetComponent<PlayerHealth>().ResetHealth();
            SpawnPlayer(i);
        }
    }

    public void CheckGameOver()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (_players[i].GetComponent<PlayerHealth>().IsDead())
            {
                deadPlayers++;
            }
        }

        if (deadPlayers == playerCount - 1)
        {
            if (!isGameOver) // Check if game over is not already triggered
            {
                isGameOver = true;
                StartCoroutine(_uiManager.ShowGameOverScreen());
            }
        }

        else
        {
            deadPlayers = 0;
        }
    }

    public void EndRound()
    {
        deadPlayers = 0;
        spawnPoints.Clear();
        for (int j = 0; j < _players.Count; j++)
        {
            if (!_players[j].GetComponent<PlayerHealth>()._isDead)
            {
                _players[j].AddScore();
            }
        }

        ClearLimbs();
        ResetGroundCheck();
        _uiManager.UpdateLeaderBoard();
        _mapManager.LoadMap();
        isGameOver = false;
    }

    void SpawnPlayer(int playerNum)
    {
        _players[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _players[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        _players[playerNum].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        _players[playerNum].GetComponent<PlayerHealth>()._isDead = false;
        _players[playerNum].transform.position = spawnPoints[playerNum].transform.position;
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public void StartGame()
    {
        _uiManager.SetPlayerCount(playerCount);
        _uiManager.SetUpHealthUI(_players);
        _uiManager.SetPlayerHealthFace(_playerConfigs);
        _uiManager.UpdatePlayerWins(_playerConfigs);

        _uiManager.SetUpLeaderBoard();
        _uiManager.UpdateLeaderBoard();

		ClearLimbs();
        startScreen = false;
        _mapManager.LoadMap();
	}
	
    public void ClearLimbs()
    {
        for (int i = 0; i < playerCount; i++)
        {
            _players[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }

    private void ResetGroundCheck()
    {
        for (int i = 0; i < playerCount; i++)
        {
            _players[i]._groundCheck.localPosition = new Vector3(0, -0.715f, 0);
        }
    }
    
}
