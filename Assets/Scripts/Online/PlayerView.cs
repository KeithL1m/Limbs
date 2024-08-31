using Unity.Netcode;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private GameObject _onlinePrefab;

    public void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        GameObject spawnedObject = Instantiate(_onlinePrefab);
        NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
        
        networkObject.SpawnWithOwnership(clientId);
    }
}
