using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerOnline : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    public GameObject Player => _player;
    private GameObject _player;

    public GameObject SpawnPlayerFirst(ulong clientID)
    {
        _player = Instantiate(_playerPrefab);
        NetworkObject networkObject = _player.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientID);
        return _player;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetCharacterServerRpc(ulong objID, int index)
    {
        SetCharacterClientRpc(objID, index);
    }

    [ClientRpc()]
    public void SetCharacterClientRpc(ulong objID, int index)
    {
        NetworkObject networkObject = NetworkManager.SpawnManager.SpawnedObjects[objID];
        PlayerConfiguration pc = ServiceLocator.Get<ConfigurationManager>().GetPlayerConfigs()[index];

        Player player = networkObject.gameObject.GetComponent<Player>();
        _player.name = $"Player {pc.PlayerIndex}";
        player.Initialize(pc);

        SpriteRenderer arrow = player.GetArrow();

        Debug.Log(pc.PlayerIndex);

        switch (pc.PlayerIndex)
        {
            case 0:
                arrow.color = new Color(1f, 0.3f, 0.3f, 0.5f);
                break;
            case 1:
                arrow.color = new Color(0.3f, 0.8f, 1f, 0.5f);
                break;
            case 2:
                arrow.color = new Color(1f, 1f, 0.3f, 0.5f);
                break;
            case 3:
                arrow.color = new Color(0.4f, 1f, 0.3f, 0.5f);
                break;
            default: break;
        }
    }

    public bool HasPrivilege()
    {
        return IsServer;
    }
}
