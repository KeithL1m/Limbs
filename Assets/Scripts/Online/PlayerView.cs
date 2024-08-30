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
        Debug.Log("test1");

        GameObject spawnedObject = Instantiate(_onlinePrefab);
        Debug.Log("test2");
        NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
        Debug.Log("test3");
        
        networkObject.SpawnWithOwnership(clientId);
    }
}
