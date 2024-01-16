using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    public int playerID;

    public static PlayerSpawnManager instance = null;



    void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("JOINED");
        Debug.Log("Player ID: " + playerInput.playerIndex);

        playerID = playerInput.playerIndex + 1;
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {

    }

}
