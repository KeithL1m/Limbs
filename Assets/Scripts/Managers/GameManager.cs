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
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    private List<Player> _players = new List<Player>();
    private List<GameObject> _spawnPoints = new List<GameObject>();

    public List<GameObject> healthUI = new List<GameObject>();
    public List<TMP_Text> winsCounter = new List<TMP_Text>();

    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private string[] gameOverMessages;

    private ConfigurationManager _configManager;
    private PauseManager _pauseManager;
    private UIManager _uiManager;

    private int _playerCount;
    public int DeadPlayers { get; set; }
    public bool StartScreen { get; private set; }

    public static GameManager instance = null;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }

        _configManager = FindObjectOfType<ConfigurationManager>();
        _playerCount = _configManager.GetPlayerNum();
        _playerConfigs = _configManager.GetPlayerConfigs();

        _uiManager = FindObjectOfType<UIManager>();
        _pauseManager = FindObjectOfType<PauseManager>();
    }

    private void Start()
    {
        for (int i = 0; i < _playerCount; i++)
        {
            _players.Add(_playerConfigs[i].Input.GetComponent<SpawnPlayer>().SpawnPlayerFirst(_playerConfigs[i]));
            

            if (_playerCount <= healthUI.Count)
            {
                GameObject newHealthUI = Instantiate(healthUI[i]);
                newHealthUI.transform.SetParent(HealthUIManager.instance.transform);
                newHealthUI.SetActive(true); // Enable the health UI for the newly joined player

                // Retrieve the Slider component from the instantiated health UI
                Slider healthSlider = newHealthUI.GetComponentInChildren<Slider>();

                // Here, you would set the health value on the player.
                _players[i].GetComponent<PlayerHealth>().SetHealthSlider(healthSlider);

                // Update the health value on the slider
                PlayerHealth playerHealth = _players[i].GetComponent<PlayerHealth>();
                float initialHealth = playerHealth._maxHealth;
                healthSlider.value = initialHealth;
            }
        }
    }

    override public void OnStart()
    {
        _pauseManager.SetCamera(FindObjectOfType<Camera>());

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            _spawnPoints.Add(gameObjects[i]);
        }

        for (int i = 0; i < _playerCount; i++)
        {
            _players[i].GetComponent<PlayerHealth>().ResetHealth();
            SpawnPlayer(i);
        }
    }

    private void Update()
    {
        if (!StartScreen)
        {
            CheckGameOver();
        }
    }


    void CheckGameOver()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].GetComponent<PlayerHealth>().IsDead())
            {
                DeadPlayers++;
            }
        }

        if (DeadPlayers == _playerCount - 1)
        {
            DeadPlayers = 0;
            _spawnPoints.Clear();
            for (int j = 0; j < _players.Count; j++)
            {
                if (!_players[j].GetComponent<PlayerHealth>()._isDead)
                {
                    _players[j].AddScore();
                }
                ClearLimbs();
            }
            _uiManager.UpdateLeaderBoard();
            MapManager.instance.LoadMap();

        }
        else
        {
            DeadPlayers = 0;
        }
    }

    void SpawnPlayer(int playerNum)
    {
        //PlayerManager.instance.AddPlayer(_players[playerNum].GetComponent<PlayerInput>());
        _players[playerNum].transform.position = _spawnPoints[playerNum].transform.position;
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        List<PlayerConfiguration> configs = new List<PlayerConfiguration>();

        for (int i = 0; i < _playerCount; i++)
        {
            configs.Add(_playerConfigs[i]);
        }

        configs.Sort((x, y) => x.Score.CompareTo(y.Score));

        return configs;
    }

    public void StartGame()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _pauseManager = FindObjectOfType<PauseManager>();

        _uiManager.SetUpLeaderBoard();
        _uiManager.UpdateLeaderBoard();

		ClearLimbs();
        StartScreen = false;
        MapManager.instance.LoadMap();
	}
	
    public void ClearLimbs()
    {
        for (int i = 0; i < _playerCount; i++)
        {
            _players[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }
}
