using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerLoadout : MonoBehaviour
{
    [SerializeField] private GameObject _playerSetupMenuPrefab;
    private PlayerConfiguration _tempConfig;

    public void Initialize(PlayerConfiguration config)
    {
        Debug.Log($"{nameof(Initialize)}");
        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);

            MenuNavegation uiInputModule = menu.GetComponentInChildren<MenuNavegation>();
            uiInputModule.Device = config.Device;
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(config.PlayerIndex);
        }
    }
}
