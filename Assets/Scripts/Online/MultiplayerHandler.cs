using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerHandler : MonoBehaviour
{
    public string JoinCode { get { return _joinCode; } private set { } }
    public bool IsHost { get { return _isHost; } private set { } }

    [SerializeField] private string _joinCode = string.Empty;
    [SerializeField] private bool _isHost = false;
    [SerializeField] public GameObject[] _dontDestroyOnLoadObject;
    [SerializeField] public PlayerView _playerView;

    private async void Start()
    {
        foreach (var item in _dontDestroyOnLoadObject)
        {
            DontDestroyOnLoad(item);
        }

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateServer()
    {
        //Creating the actual server
        try
        {
            _isHost = true;
            const int maxPlayersInServer = 3;//Does not count the host
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersInServer);

            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.OnClientConnectedCallback += _playerView.OnClientConnected;

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
        _isHost = false;

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
}
