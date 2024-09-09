using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerLoadout : NetworkBehaviour
{
    private GameLoader _loader = null;

    [SerializeField] private GameObject _playerSetupMenuPrefab;
    private PlayerConfiguration _tempConfig;

    public void Initialize(PlayerConfiguration config, int playerInArrayIndex)
    {
        Debug.Log($"{nameof(Initialize)}");

        if (NetworkManager)
        {
            _tempConfig = config;
            SpawnObjectInWebServerRpc(NetworkManager.Singleton.LocalClientId, playerInArrayIndex);
            return;
        }

        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);

            MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
            uiInputModule.Device = config.Device;
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(config.PlayerIndex, playerInArrayIndex);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjectInWebServerRpc(ulong id, int playerInArrayIndex)
    {
        var rootMenu = GameObject.Find("Loadout");

        var menu = Instantiate(_playerSetupMenuPrefab);
        NetworkObject networkObject = menu.GetComponent<NetworkObject>();

        networkObject.SpawnWithOwnership(id);
        menu.transform.SetParent(rootMenu.transform, false);

        SetControllerForCreatedEntityClientRpc(networkObject.NetworkObjectId, id, playerInArrayIndex);
    }

    [ClientRpc]
    private void SetControllerForCreatedEntityClientRpc(ulong networkObjectId, ulong clientId, int playerInArrayIndex)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var networkObject))
            {
                var menu = networkObject.gameObject;

                MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
                uiInputModule.Device = _tempConfig.Device;
                menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_tempConfig.PlayerIndex, playerInArrayIndex);
            }
        }
    }
}
