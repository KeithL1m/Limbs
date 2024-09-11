using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigManagerLocal : MonoBehaviour
{
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    [SerializeField] private List<Sprite> _playerNums;

    public bool InLoadout { get; set; } = false;

    private int _playerNum = 0;

    public ConfigManagerLocal Initialize()
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

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!InLoadout)
            return;
        if (!_playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            Debug.Log("Player Has Joined");
            pi.transform.SetParent(transform);
            _playerConfigs.Add(new PlayerConfiguration(pi));
            _playerConfigs[_playerNum].Num = _playerNums[_playerNum];
            _playerNum++;
        }
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

