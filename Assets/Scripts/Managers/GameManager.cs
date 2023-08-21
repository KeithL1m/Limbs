using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Manager
{
    private PlayerInputManager playerManager;
    public List<PlayerInput> playerList = new List<PlayerInput>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    private int playerCount;

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

        playerManager = PlayerInputManager.instance;

        playerManager.onPlayerJoined += OnPlayerJoined;
        playerManager.onPlayerLeft += OnPlayerLeft;

        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);
    }

    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            spawnPoints.Add(gameObjects[i]);
        }
    }

    override public void OnStart()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            spawnPoints.Add(gameObjects[i]);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            SpawnPlayer(i);
        }
    }

    private void Update()
    {
        if (startScreen)
        {
            if (Input.GetKeyDown(KeyCode.Return) && playerList.Count > 1)
            {
                startScreen = false;
                MapManager.instance.LoadMap();
            }
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("JOINED");
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
        playerManager.JoinPlayerFromActionIfNotAlreadyJoined(context);
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

    void SpawnPlayer(int playerNum)
    {
        playerList[playerNum].transform.position = spawnPoints[playerNum].transform.position;
    }
}
