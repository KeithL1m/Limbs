using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Manager
{
    public List<PlayerInput> playerList = new List<PlayerInput>();
    public List<GameObject> spawnPoints = new List<GameObject>();

    [SerializeField] private Button button;
    [SerializeField] private EventSystem system;
    private PauseManager pauseManager;

    private int playerCount;
    public int deadPlayers;

    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;

    public static GameManager instance = null;

    public event System.Action<PlayerInput> PlayerJoinedGame;
    public event System.Action<PlayerInput> PlayerLeftGame;


    bool startScreen = true;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }

        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;

        pauseManager = FindObjectOfType<PauseManager>();

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
        if (startScreen)
        {
            if (playerList.Count > 1)
            {
                button.gameObject.SetActive(true);
                system.SetSelectedGameObject(button.gameObject);
            }
        }
        else
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
                playerList[j].GetComponent<PlayerLimbs>().ClearLimbs();
            }
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

    public void StartGame()
    {
        startScreen = false;
        MapManager.instance.LoadMap();
    }

    public int GetPlayerCount()
    {
        return playerList.Count;
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
}
