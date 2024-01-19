using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

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
            var menu = Instantiate(_playerSetupMenuPrefab, rootMenu.transform);
            _input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupController>().SetPlayerIndex(_input.playerIndex);
        }
    }
}
