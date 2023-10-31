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

    private ConfigurationManager _configManager;

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();
    public List<Player> _players = new List<Player>();

    private float originalTimeScale = 1.0f;

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

        //_configManager = ServiceLocator.Get<ConfigurationManager>();
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

        for (int i = 0; i < playerCount; i++)
        {
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
                StartCoroutine(ShowGameOverScreen());
            }
        }

        else
        {
            deadPlayers = 0;
        }
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
        List<PlayerConfiguration> configs = new List<PlayerConfiguration>();

        for (int i = 0; i < playerCount; i++)
        {
            configs.Add(_playerConfigs[i]);
        }

        configs.Sort((x, y) => y.Score.CompareTo(x.Score));

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

    private IEnumerator ShowGameOverScreen()
    {
        Time.timeScale = 0.5f;
        gameOverBG.SetActive(true);

        // Display a random game over message from the array
        int randomMessageIndex = Random.Range(0, gameOverMessages.Length);
        gameOverText.text = gameOverMessages[randomMessageIndex];

        Color randomColor = new Color(Random.value, Random.value, Random.value);
        gameOverText.color = randomColor;

        // Wait for 5 seconds
        float endTime = Time.realtimeSinceStartup + 5.0f;

        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }

        Time.timeScale = originalTimeScale;


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

        gameOverBG.SetActive(false);
        isGameOver = false;
    }
}
