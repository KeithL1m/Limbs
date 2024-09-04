using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private GameObject _localGameLoader;
    [SerializeField] private GameObject _onlineGameLoader;
    [SerializeField] private GameObject _firstButton;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }

    public void InstantiateLocalLoader()
    {
        Instantiate(_localGameLoader);
    }

    public void InstantiateOnlineLoader()
    {
        Instantiate(_onlineGameLoader);
    }
}
