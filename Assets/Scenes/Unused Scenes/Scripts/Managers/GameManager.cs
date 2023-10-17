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

    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private string[] gameOverMessages;

    [SerializeField] private Button button;
    [SerializeField] private EventSystem system;

    private PauseManager pauseManager;
    private UIManager uiManager;

    private bool isGameOver = false;
    private float originalTimeScale = 1.0f;

    private int playerCount;
    public int deadPlayers;

    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;

    public static GameManager instance = null;

    public event System.Action<PlayerInput> PlayerJoinedGame;
    public event System.Action<PlayerInput> PlayerLeftGame;


    public bool startScreen = true;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }

        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;

        originalTimeScale = Time.timeScale;

        pauseManager = FindObjectOfType<PauseManager>();
        uiManager = FindObjectOfType<UIManager>();

        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);
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
            playerList[i].GetComponent<PlayerHealth>().ResetHealth();
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
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<PlayerHealth>().IsDead())
            {
                deadPlayers++;
            }
        }

        if (deadPlayers == playerCount - 1)
        {
            deadPlayers = 0;
            spawnPoints.Clear();
            for (int j = 0; j < playerList.Count; j++)
            {
                if (!playerList[j].GetComponent<PlayerHealth>()._isDead)
                {
                    playerList[j].GetComponent<Player>().AddScore();
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
        PlayerManager.instance.AddPlayer(playerList[playerNum].GetComponent<PlayerInput>());
        playerList[playerNum].transform.position = spawnPoints[playerNum].transform.position;
    }

    //player joining/leaving functions

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerList.Add(playerInput);
        DontDestroyOnLoad(playerList[playerCount]);
        playerCount++;



        if (PlayerJoinedGame != null)
        {
            PlayerJoinedGame(playerInput);
        }

        if (playerCount <= healthUI.Count)
        {
            GameObject newHealthUI = Instantiate(healthUI[playerCount - 1]);
            newHealthUI.transform.SetParent(HealthUIManager.instance.transform);
            newHealthUI.SetActive(true); // Enable the health UI for the newly joined player

            // Retrieve the Slider component from the instantiated health UI
            Slider healthSlider = newHealthUI.GetComponentInChildren<Slider>();

            // Here, you would set the health value on the player.
            playerInput.GetComponent<PlayerHealth>().SetHealthSlider(healthSlider); 

            // Update the health value on the slider
            PlayerHealth playerHealth = playerInput.GetComponent<PlayerHealth>();
            float initialHealth = playerHealth._maxHealth;
            healthSlider.value = initialHealth;
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {

    }

    void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
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

    private IEnumerator ShowGameOverScreen()
    {
        // Slow down the game to 0.5x
        Time.timeScale = 0.5f;

        // Show the UI panel
        gameOverBG.SetActive(true);

        // Display a random game over message from the array
        int randomMessageIndex = Random.Range(0, gameOverMessages.Length);
        gameOverText.text = gameOverMessages[randomMessageIndex];

        // Wait for 5 seconds
        float endTime = Time.realtimeSinceStartup + 5.0f;

        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }

        deadPlayers = 0;
        spawnPoints.Clear();
    }

        public List<PlayerData> GetPlayerDatas()
    {
        List<PlayerData> playerDatas = new List<PlayerData>();

        for (int i = 0; i < playerList.Count; i++)
        {
            playerDatas.Add(playerList[i].GetComponent<PlayerData>());
        }

        playerDatas.Sort((x, y) => x.score.CompareTo(y.score));

        return playerDatas;
    }

    public void ClearLimbs()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }
}
