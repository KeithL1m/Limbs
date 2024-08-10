using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class MultiplayerHandler : MonoBehaviour
{
    private string _joinCode = string.Empty;

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

    private async void CreateServer()
    {
        //Creating the actual server
        try
        {
            //Does not count the host
            const int maxPlayersInServer = 3;
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersInServer);

            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"<color=red>{ex}</color>");
        }
    }

    private async void JoinServer(string joinCode)
    {
        try
        {
            await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"<color=red>{ex}</color>");
        }
    }
}
