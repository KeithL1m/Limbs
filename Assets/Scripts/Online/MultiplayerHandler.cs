using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerHandler : NetworkBehaviour
{
    public string JoinCode { get { return _joinCode; } private set { } }

    [Space, Header("Server")]
    [SerializeField] private string _joinCode = string.Empty;

    [SerializeField] private GameObject _playerConfig;

    [SerializeField] private ConfigurationManager _configManager;
    private PlayerInput _tempPlayerInputHolder;
    //temp
    [SerializeField] private OnlineSettings _visualSettings;

    private void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private async void Initialize()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized || UnityServices.State == ServicesInitializationState.Initializing)
        {
            return;
        }

        {
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<MultiplayerHandler>(gameObject.GetComponent<MultiplayerHandler>());

            var gmObj = ServiceLocator.Get<GameManager>().gameObject;
            PlayerInputManager pi = gmObj.GetComponent<PlayerInputManager>();
            pi.playerPrefab = null;

            _configManager = gmObj.GetComponent<ConfigurationManager>();
        }

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await CreateServer();
        //TODO: shouldn't be called here
        _visualSettings.SetStrg(_joinCode);
    }

    public async Task<string> CreateServer()
    {
        //Creating the actual server
        try
        {
            const int maxPlayersInServer = 3;//Does not count the host
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersInServer);

            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

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
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
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

    public void OnClientConnected(PlayerInput pi)
    {
        if (IsServer)
        {
            OnClientConnectedServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        _tempPlayerInputHolder = pi;
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnClientConnectedServerRpc(ulong id)
    {
        GameObject newObj = Instantiate(_playerConfig);

        NetworkObject networkObject = newObj.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(id);
        NotifyClientOfNewObjectClientRpc(networkObject.NetworkObjectId);
    }

    [ClientRpc]
    private void NotifyClientOfNewObjectClientRpc(ulong networkObjectId)
    {
        // Find the new object using the network object ID
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var networkObject))
        {
            var newObject = networkObject.gameObject.GetComponent<PlayerInput>();
            newObject = _tempPlayerInputHolder;
            _tempPlayerInputHolder = null;

            _configManager.AddNewNetworkPlayer(newObject.GetComponent<PlayerInput>());
        }
    }
}
