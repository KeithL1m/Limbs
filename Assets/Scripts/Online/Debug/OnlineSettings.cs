using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSettings : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _handler;
    [SerializeField] private TMP_InputField _joinInputField;

    public async void CreateServer()
    {
        _joinInputField.text = await _handler.CreateServer();
    }

    public void JoinServer()
    {
        _handler.JoinServer(_joinInputField.text);
    }

    public void SetStrg(string s)
    {
        _joinInputField.text = s;
    }
}
