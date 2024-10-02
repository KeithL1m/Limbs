using TMPro;
using UnityEngine;

public class OnlineSettings : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _handler;
    [SerializeField] private TMP_InputField _joinInputField;

    private void Awake()
    {
        _handler = ServiceLocator.Get<MultiplayerHandler>();
    }

    public async void CreateServer()
    {
        _joinInputField.text = await _handler.CreateServer();
    }

    public void JoinServer()
    {
        _handler.JoinServer(_joinInputField.text);
    }
}
