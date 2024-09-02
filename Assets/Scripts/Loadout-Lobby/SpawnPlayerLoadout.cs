using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerLoadout : MonoBehaviour
{
    private GameLoader _loader = null;

    [SerializeField] private GameObject _playerSetupMenuPrefab;
    [SerializeField] private PlayerInput _input;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab);
            NetworkObject networkObject = menu.GetComponent<NetworkObject>();
            
            networkObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            menu.transform.SetParent(rootMenu.transform, false);

            _input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_input.playerIndex);
        }
    }
}
