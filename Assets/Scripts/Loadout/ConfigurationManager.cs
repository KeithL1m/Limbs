using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    private int _playerNum;

    private void Awake()
    {
        DontDestroyOnLoad(this);
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

        //if (_playerConfigs.All(p => p.IsReady == true))
        //{
            SceneManager.LoadScene(2);
        //}
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!_playerConfigs.Any(p =>p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            _playerConfigs.Add(new PlayerConfiguration(pi));
            _playerNum++;
        }
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
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input {get; set;}
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }

    public int Score { get; set; }
    public Sprite Head { get; set; }
    public Sprite Body { get; set; }
    public string Name { get; set; }
}
