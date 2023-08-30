using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Manager
{
    public List<PlayerInput> playerList = new List<PlayerInput>();
    public List<GameObject> spawnPoints = new List<GameObject>();
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


        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);
    }

    private void Start()
    {

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
            playerList[i].GetComponent<PlayerHealth>().ResetHealth();
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

        if (deadPlayers == playerCount-1)
        {
            deadPlayers = 0;
            spawnPoints.Clear();
            MapManager.instance.LoadMap();
        }
        else
        {
            deadPlayers = 0;
        }
    }

    void SpawnPlayer(int playerNum)
    {
        playerList[playerNum].GetComponent<Player>().ClearLimbs();
        playerList[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        playerList[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        playerList[playerNum].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        playerList[playerNum].transform.position = spawnPoints[playerNum].transform.position;
        playerList[playerNum].GetComponent<PlayerHealth>()._isDead = false;

        PlayerManager.instance.AddPlayer(playerList[playerNum].GetComponent<Player>());
    }

    //player joining/leaving functions

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

}
