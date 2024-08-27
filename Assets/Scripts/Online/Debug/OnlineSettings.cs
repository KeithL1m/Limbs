using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSettings : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _handler;
    [SerializeField] private InputField _textMeshProUGUI;

    private void Start()
    {
        if (_handler == null)
        {
        }
    }

    public async void SetServer()
    {
        _textMeshProUGUI.text = await _handler.CreateServer();
    }

    public void SetText()
    {
        _handler.JoinServer(_textMeshProUGUI.text);
    }
}
