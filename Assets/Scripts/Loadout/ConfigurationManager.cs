using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    public void SetPlayerHead(int index, Sprite head)
    {
        _playerConfigs[index].PData.PlayerHead = head;
    }

    public void SetPlayerBody(int index, Sprite body)
    {
        _playerConfigs[index].PData.PlayerBody = body;
    }

    public void SetPlayerName(int index, string name)
    {
        _playerConfigs[index].PData.PlayerName = name;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;

        if (_playerConfigs.All(p => p.IsReady == true) && _playerConfigs.Count > 1)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!_playerConfigs.Any(p =>p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            _playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
        PData = new PlayerData();
    }
    public PlayerInput Input {get; set;}
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public PlayerData PData { get; set; }
}

public class PlayerData
{
    public int Score;
    public string PlayerName;
    public Sprite PlayerHead;
    public Sprite PlayerBody;
}
