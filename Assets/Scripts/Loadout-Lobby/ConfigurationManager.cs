using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    [SerializeField] private List<Sprite> _playerNums;

    public bool InLoadout { get; set; } = false;

    private int _playerNum = 0;

    public ConfigurationManager Initialize()
    {
        Debug.Log("Loading Configuration Manager");
        return this;
    }

    public void SetPlayerHead(int index, Sprite head)
    {
        _playerConfigs[index].Head = head;
    }

    public void SetPlayerBody(int index, Sprite body)
    {
        _playerConfigs[index].Body = body;
    }

    public void SetPlayerName(int index, string name)
    {
        _playerConfigs[index].Name = name;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;

        if (_playerConfigs.All(p => p.IsReady == true) && _playerNum > 1)
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(5);
        //NetworkManager.Singleton.SceneManager.LoadScene("YourSceneName", LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public bool HandlePlayerJoin(InputDevice device, GameObject prefab)
    {
        if (!InLoadout)
        {
            return false;
        }

        bool hasDevice = _playerConfigs.Any(p => p.Device == device);
        if (!hasDevice)
        {
            Debug.Log("Player Has Joined");

            var player = Instantiate(prefab, transform);
            JoinPlayer(player, device, _playerNum);
            ++_playerNum;
            return true;
        }

        return false;
    }

    public void JoinPlayer(GameObject player, InputDevice device, int playerNum)
    {
        _playerNum = playerNum;
        _playerConfigs.Add(new PlayerConfiguration(device, player, _playerNum));
        _playerConfigs.Last().Num = _playerNums[_playerNum];

        var spawnMenu = player.GetComponent<SpawnPlayerLoadout>();
        spawnMenu.Initialize(_playerConfigs.Last());
    }

    public void ResetConfigs()
    {
        _playerConfigs.Clear();
        _playerNum = 0;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public int GetPlayerNum()
    {
        return _playerNum;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(InputDevice device, GameObject gObj, int playerIndex)
    {
        PlayerConfigObject = gObj;
        Device = device;
        PlayerIndex = playerIndex;
    }
    public int PlayerIndex = -1;
    public GameObject PlayerConfigObject;
    public InputDevice Device { get; set; }
    public bool IsReady { get; set; }

    public int Score { get; set; }
    public Sprite Head { get; set; }
    public Sprite Body { get; set; }
    public Sprite Num { get; set; }
    public string Name { get; set; }
}
