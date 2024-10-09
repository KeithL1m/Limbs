using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigurationManagerOnline : NetworkBehaviour
{
    public IReadOnlyList<PlayerConfiguration> PlayerConfigs => _playerConfigs.AsReadOnly();
    private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

    [SerializeField] private List<Sprite> _playerNums;

    private int _playerNum = 0;

    public ConfigurationManagerOnline Initialize()
    {
        Debug.Log("Loading Configuration Manager");
        return this;
    }

    public void SetPlayerName(int index, string name)
    {
        _playerConfigs[index].Name = name;
    }

    public void SetPlayerHead(int index, Sprite head)
    {
        _playerConfigs[index].Head = head;
    }

    public void SetPlayerBody(int index, Sprite body)
    {
        _playerConfigs[index].Body = body;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;

        if (_playerConfigs.All(p => p.IsReady == true) && _playerNum > 1)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Meatcase", LoadSceneMode.Single);
        }
    }

    void LoadSceneAsync()
    {
        //TODO: Maybe check how LoadSceneMode.Additive works and make this function
        //while (!asyncLoad.isDone)
        //{
        //    yield return null;
        //}
    }

    public void HandlePlayerJoin(InputDevice device)
    {
        if (SceneManager.GetActiveScene().name != "z_LoadoutMultiplayer" || !NetworkManager)
        {
            return;
        }

        bool hasDevice = _playerConfigs.Any(p => p.Device == device);
        if (!hasDevice)
        {
            Debug.Log("Player Has Joined");

            var network = ServiceLocator.Get<MultiplayerHandler>();
            network.OnClientConnected(device);
        }
    }

    public void JoinPlayer(GameObject player, InputDevice device)
    {
        _playerConfigs.Add(new PlayerConfiguration(device, player, _playerNum));
        _playerConfigs.Last().Num = _playerNums[_playerNum];
        ++_playerNum;
    }

    public void SpawnMenu(GameObject player)
    {
        var spawnMenu = player.GetComponent<SpawnLoadoutOnline>();
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
