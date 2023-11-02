using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameManager : Manager
{
    public List<PlayerInput> playerList = new List<PlayerInput>();
    public List<GameObject> spawnPoints = new List<GameObject>();

    private ConfigurationManager _configManager;

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private PauseManager pauseManager;
    private UIManager uiManager;
    private bool isGameOver = false;

    private int playerCount;
    public int deadPlayers;

    public static GameManager instance = null;

    public bool startScreen = true;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }

        _configManager = FindObjectOfType<ConfigurationManager>();
        playerCount = _configManager.GetPlayerNum();
        _playerConfigs = _configManager.GetPlayerConfigs();

        uiManager = FindObjectOfType<UIManager>();
        pauseManager = FindObjectOfType<PauseManager>();
    }

    private void Start()
    {
        Debug.Log(playerCount);
        for (int i = 0; i < playerCount; i++)
        {
            _players.Add(_playerConfigs[i].Input.GetComponent<SpawnPlayer>().SpawnPlayerFirst(_playerConfigs[i]));
            playerList.Add(_playerConfigs[i].Input);
        }

        uiManager.SetPlayerCount(playerCount);
        uiManager.SetUpHealthUI(_players);
        uiManager.SetPlayerHealthFace(_playerConfigs);
        uiManager.UpdatePlayerWins(_playerConfigs);
    }

    override public void OnStart()
    {
        pauseManager.SetCamera(FindObjectOfType<Camera>());

        uiManager.UpdatePlayerWins(_playerConfigs);

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
                StartCoroutine(uiManager.ShowGameOverScreen());
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
        uiManager.UpdateLeaderBoard();
        MapManager.instance.LoadMap();
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
        uiManager.SetUpLeaderBoard();
        uiManager.UpdateLeaderBoard();

		ClearLimbs();
        startScreen = false;
        MapManager.instance.LoadMap();
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
