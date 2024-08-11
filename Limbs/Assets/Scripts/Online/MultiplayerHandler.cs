using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class MultiplayerHandler : MonoBehaviour
{
    [SerializeField] private string _joinCode = string.Empty;

    private async void Start()
    {
        //Creating the instance of server and checking that this player gets an ID
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
            //Does not count the host
            const int maxPlayersInServer = 3;
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
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"<color=red>{ex}</color>");
        }
    }
}
