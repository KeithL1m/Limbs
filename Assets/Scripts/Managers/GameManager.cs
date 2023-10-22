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
    public List<GameObject> healthUI = new List<GameObject>();
    public List<TMP_Text> winsCounter = new List<TMP_Text>();

    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private string[] gameOverMessages;

    [SerializeField] private Button button;
    [SerializeField] private EventSystem system;
    private ConfigurationManager _configManager;

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private PauseManager pauseManager;
    private UIManager uiManager;

    private int playerCount;
    public int deadPlayers;

    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;

    public static GameManager instance = null;

    public event System.Action<PlayerInput> PlayerLeftGame;


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
        for (int i = 0; i < playerCount; i++)
        {
            _players.Add(_playerConfigs[i].Input.GetComponent<SpawnPlayer>().SpawnPlayerFirst(_playerConfigs[i]));
            playerList.Add(_playerConfigs[i].Input);


            if (playerCount <= healthUI.Count)
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
        pauseManager.SetCamera(FindObjectOfType<Camera>());

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            spawnPoints.Add(gameObjects[i]);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            _players[i].GetComponent<PlayerHealth>().ResetHealth();
            SpawnPlayer(i);
        }
    }

    private void Update()
    {
        if (!startScreen)
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
                deadPlayers++;
            }
        }

        if (deadPlayers == playerCount - 1)
        {
            deadPlayers = 0;
            spawnPoints.Clear();
            for (int j = 0; j < _players.Count; j++)
            {
                if (!_players[j].GetComponent<PlayerHealth>()._isDead)
                {
                    _players[j].AddScore();
                }
                ClearLimbs();
            }
            uiManager.UpdateLeaderBoard();
            MapManager.instance.LoadMap();

        }
        else
        {
            deadPlayers = 0;
        }
    }

    void SpawnPlayer(int playerNum)
    {
        PlayerManager.instance.AddPlayer(_players[playerNum]);
        playerList[playerNum].transform.position = spawnPoints[playerNum].transform.position;
    }

    //player joining/leaving functions
    public void OnPlayerLeft(PlayerInput playerInput)
    {

    }

    void LeaveAction(InputAction.CallbackContext context)
    {
        Debug.Log("We leaving");
        if (playerList.Count > 1)
        {
            foreach (var player in playerList)
            {
                foreach(var device in player.devices)
                {
                    if (device != null && context.control.device == device)
                    {
                        UnregisterPlayer(player);
                        return;
                    }
                }
            }
        }
    }

    void UnregisterPlayer(PlayerInput playerInput)
    {
        playerList.Remove(playerInput);

        if (PlayerLeftGame != null)
        {
            PlayerLeftGame(playerInput);
        }

        Destroy(playerInput.transform.gameObject);
    }

    public int GetPlayerCount()
    {
        return playerList.Count;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        List<PlayerConfiguration> configs = new List<PlayerConfiguration>();

        for (int i = 0; i < playerList.Count; i++)
        {
            configs.Add(_playerConfigs[i]);
        }

        configs.Sort((x, y) => x.Score.CompareTo(y.Score));

        return configs;
    }

    public void StartGame()
    {
        uiManager = FindObjectOfType<UIManager>();
        pauseManager = FindObjectOfType<PauseManager>();

        uiManager.SetUpLeaderBoard();
        uiManager.UpdateLeaderBoard();

		ClearLimbs();
        startScreen = false;
        MapManager.instance.LoadMap();
	}
	
    public void ClearLimbs()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            _players[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }
}
