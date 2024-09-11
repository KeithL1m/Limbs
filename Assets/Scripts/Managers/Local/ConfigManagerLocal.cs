using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigManagerLocal : ConfigurationManagerBase
{
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    [SerializeField] private List<Sprite> _playerNums;

    public override bool InLoadout { get; set; }

    private int _playerNum = 0;

    public override ConfigurationManagerBase Initialize()
    {
        Debug.Log("Loading Configuration Manager");
        return this;
    }

    public override void SetPlayerHead(int index, Sprite head)
    {
        _playerConfigs[index].Head = head;
    }

    public override void SetPlayerBody(int index, Sprite body)
    {
        _playerConfigs[index].Body = body;
    }

    public override void SetPlayerName(int index, string name)
    {
        _playerConfigs[index].Name = name;
    }

    public override void ReadyPlayer(int index)
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

    public override bool HandlePlayerJoin(InputDevice device, GameObject prefab)
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

    public override void JoinPlayer(GameObject player, InputDevice device, int playerNum)
    {
        _playerConfigs.Add(new PlayerConfiguration(device, player, _playerNum));
        _playerConfigs.Last().Num = _playerNums[_playerNum];

        var spawnMenu = player.GetComponent<SpawnPlayerLoadout>();
        spawnMenu.Initialize(_playerConfigs.Last(), _playerConfigs.Count - 1);
    }

    public override void ResetConfigs()
    {
        _playerConfigs.Clear();
        _playerNum = 0;
    }

    public override List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public override int GetPlayerNum()
    {
        return _playerNum;
    }
}

