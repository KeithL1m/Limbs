using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    //private int _maxPlayers = 4;

    public void SetPlayerHead(int index, Sprite head)
    {

    }

    public void SetPlayerBody(int index, Sprite body)
    {

    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;

        if (_playerConfigs.All(p => p.IsReady == true))
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
    }
    public PlayerInput Input {get; set;}
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
}
