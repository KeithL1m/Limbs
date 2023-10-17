using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerLoadout : MonoBehaviour
{
    [SerializeField] private GameObject _playerSetupMenuPrefab;
    [SerializeField] private PlayerInput _input;

    private void Awake()
    {
        var rootMenu = GameObject.Find("Loadout");

        if (rootMenu != null)
        {
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);
            _input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_input.playerIndex);
        }
    }
}
