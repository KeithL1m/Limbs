using Unity.Netcode;
using UnityEngine;

public class SpawnLoadoutOnline : NetworkBehaviour
{
    [SerializeField] private GameObject _playerSetupMenuPrefab;
    private PlayerConfiguration _tempConfig;

    public void Initialize(PlayerConfiguration config)
    {
        Debug.Log($"{nameof(Initialize)}");

        _tempConfig = config;
        SpawnObjectInWebServerRpc(NetworkManager.Singleton.LocalClientId, config.PlayerIndex);
        return;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjectInWebServerRpc(ulong id, int playerIndex)
    {
        var rootMenu = GameObject.Find("Loadout");

        var menu = Instantiate(_playerSetupMenuPrefab);
        NetworkObject networkObject = menu.GetComponent<NetworkObject>();

        networkObject.SpawnWithOwnership(id);
        menu.transform.SetParent(rootMenu.transform, false);

        SetControllerForCreatedEntityClientRpc(networkObject.NetworkObjectId, id, playerIndex);
    }

    [ClientRpc]
    private void SetControllerForCreatedEntityClientRpc(ulong networkObjectId, ulong clientId, int playerIndex)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var networkObject))
        {
            var menu = networkObject.gameObject;

            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
                uiInputModule.Device = _tempConfig.Device;
                menu.GetComponent<SetupControllerOnline>().SetPlayerIndex(playerIndex);
            }
            else
            {
                menu.GetComponent<SetupControllerOnline>().SetPlayerIndex(playerIndex);
            }
        }
    }
}
