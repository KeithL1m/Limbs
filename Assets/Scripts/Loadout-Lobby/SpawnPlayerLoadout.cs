using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerLoadout : NetworkBehaviour
{
    private GameLoader _loader = null;

    [SerializeField] private GameObject _playerSetupMenuPrefab;

    private InputAction _moveAction;
    private InputAction _submitAction;
    private InputAction _cancelAction;

    public void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        if (ServiceLocator.Get<MultiplayerHandler>())
        {
            SpawnObjectInWebServerRpc(NetworkManager.Singleton.LocalClientId);
            return;
        }

        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);

            var uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            //uiInputModule.

            //menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_input.playerIndex);
        }
    }

    private void SetDeviceForUI(InputDevice device, GameObject menu)
    {
        //var uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
        //uiInputModule.move.
        //_moveAction = new InputAction(type: InputActionType.PassThrough, binding: "<Gamepad>/leftStick");
        //_moveAction.AddBinding($"<Gamepad>{device.name}/leftStick");
        //
        //_submitAction = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonSouth");
        //_submitAction.AddBinding($"<Gamepad>{device.name}/buttonSouth");
        //
        //_cancelAction = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonEast");
        //_cancelAction.AddBinding($"<Gamepad>{device.name}/buttonEast");
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjectInWebServerRpc(ulong id)
    {
        var rootMenu = GameObject.Find("Loadout");

        var menu = Instantiate(_playerSetupMenuPrefab);
        NetworkObject networkObject = menu.GetComponent<NetworkObject>();

        networkObject.SpawnWithOwnership(id);
        menu.transform.SetParent(rootMenu.transform, false);

        //_input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
        //menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_input.playerIndex);
    }
}
