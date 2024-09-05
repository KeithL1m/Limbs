using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerLoadout : NetworkBehaviour
{
    private GameLoader _loader = null;

    [SerializeField] private GameObject _playerSetupMenuPrefab;
    private PlayerConfiguration _tempConfig;

    public void Initialize(PlayerConfiguration config)
    {
        Debug.Log($"{nameof(Initialize)}");

        if (ServiceLocator.Get<MultiplayerHandler>())
        {
            _tempConfig = config;
            SpawnObjectInWebServerRpc(NetworkManager.Singleton.LocalClientId);
            return;
        }

        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);

            MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
            uiInputModule.Device = _tempConfig.Device;
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(config.PlayerIndex);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjectInWebServerRpc(ulong id)
    {
        var rootMenu = GameObject.Find("Loadout");

        var menu = Instantiate(_playerSetupMenuPrefab);
        NetworkObject networkObject = menu.GetComponent<NetworkObject>();

        networkObject.SpawnWithOwnership(id);
        menu.transform.SetParent(rootMenu.transform, false);

        SetControllerForCreatedEntityClientRpc(networkObject.NetworkObjectId, id);
    }

    [ClientRpc]
    private void SetControllerForCreatedEntityClientRpc(ulong networkObjectId, ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var networkObject))
            {
                var menu = networkObject.gameObject;

                MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
                uiInputModule.Device = _tempConfig.Device;
                menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_tempConfig.PlayerIndex);
            }
        }
    }
}
