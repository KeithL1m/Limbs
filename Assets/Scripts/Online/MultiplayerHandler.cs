using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class MultiplayerHandler : NetworkBehaviour
{
    [Header("Server")]
    [SerializeField] private string _joinCode = string.Empty;
    public string JoinCode { get { return _joinCode; } private set { } }

    [SerializeField] private GameObject _playerConfigObj;
    [SerializeField] private GameObject _networkManagerPrefab;

    [Space, Header("Managers")]
    private GameManager _gameManager;
    private ConfigurationManagerOnline _configManager;
    private int _playerCount = 0;

    [Space, Header("Holders")]
    private InputDevice _tempDevice;
    private GameObject _networkManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        _configManager = GetComponent<ConfigurationManagerOnline>();

        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(() =>
        {
            ServiceLocator.Register<MultiplayerHandler>(gameObject.GetComponent<MultiplayerHandler>());
        });
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async void StartMultiplayer()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized || UnityServices.State == ServicesInitializationState.Initializing)
        {
            return;
        }

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await CreateServer();
    }

    public async Task<string> CreateServer()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.Shutdown();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        //Creating the actual server
        try
        {
            const int maxPlayersInServer = 3;//Does not count the host
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersInServer);

            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;

            return _joinCode;
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"<color=red>{ex}</color>");
            return ex.ToString();
        }
    }

    public async void JoinServer(string joinCode)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.Shutdown();
        }
        else if(NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            _joinCode = joinCode;
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"<color=red>{ex}</color>");
        }
    }

    public void OnClientConnected(InputDevice device)
    {
        _tempDevice = device;
        OnClientConnectedServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnClientConnectedServerRpc(ulong id)
    {
        if (_playerCount >= 4)
        {
            return;
        }

        GameObject newObj = Instantiate(_playerConfigObj);

        NetworkObject networkObject = newObj.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(id);
        networkObject.TrySetParent(_configManager.GetComponent<NetworkObject>());

        NotifyClientOfNewObjectClientRpc(networkObject.NetworkObjectId, id, _playerCount);
        ++_playerCount;
    }

    [ClientRpc]
    private void NotifyClientOfNewObjectClientRpc(ulong networkObjectId, ulong clientId, int playerNum)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var networkObject))
        {
            var target = networkObject.gameObject;
            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                _configManager.JoinPlayer(target, _tempDevice);
                _configManager.SpawnMenu(target);
                _tempDevice = null;
            }
            else
            {
                _configManager.JoinPlayer(target, null);
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartMenu":
            case "MainMenu":
            case "z_Loadout":
                {
                    if (NetworkManager && (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient))
                    {
                        if(NetworkManager.Singleton.IsHost)
                        {
                            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                        }
                        NetworkManager.Singleton.Shutdown();
                        Destroy(_networkManagerPrefab);
                        _networkManager = null;
                    }
                    _gameManager.IsOnline = false;
                    break;
                }
            case "z_LoadoutMultiplayer":
                {
                    if (!_networkManager)
                    {
                        _networkManager = Instantiate(_networkManagerPrefab);
                        DontDestroyOnLoad(_networkManager);
                    }
                    StartMultiplayer();
                    _gameManager.IsOnline = true;
                    break;
                }
        }
    }
    private void HandleClientConnected(ulong clientId)
    {
        NotifyAllClientsOfNewClientServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyAllClientsOfNewClientServerRpc(ulong clientId)
    {
        ulong[] networkObjectIds = new ulong[_configManager.PlayerConfigs.Count];
        for (int i = 0; i < _configManager.PlayerConfigs.Count; ++i)
        {
            NetworkObject networkObject = _configManager.PlayerConfigs[i].PlayerConfigObject.GetComponent<NetworkObject>();
            networkObjectIds[i] = networkObject.NetworkObjectId;
        }
        NotifyAllClientsOfNewClientClientRpc(clientId, networkObjectIds);
    }

    [ClientRpc]
    private void NotifyAllClientsOfNewClientClientRpc(ulong clientId, ulong[] gameObjects)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            foreach (ulong objID in gameObjects)
            {
                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objID, out NetworkObject networkObject))
                {
                    _configManager.JoinPlayer(networkObject.gameObject, null);
                }
            }
        }
    }
}
