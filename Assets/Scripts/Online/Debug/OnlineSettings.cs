using TMPro;
using UnityEngine;

public class OnlineSettings : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _handler;
    [SerializeField] private TMP_InputField _joinInputField;

    [SerializeField] private GameObject[] _serverButtons = new GameObject[2];

    private bool IsTryingToJoin = false;

    private void Awake()
    {
        _handler = ServiceLocator.Get<MultiplayerHandler>();
    }

    public async void CreateServer()
    {
        _joinInputField.text = await _handler.CreateServer();
        _joinInputField.gameObject.SetActive(true);

        foreach (var button in _serverButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void JoinServer()
    {
        if (IsTryingToJoin)
        {
            _handler.JoinServer(_joinInputField.text);
            _serverButtons[1].gameObject.SetActive(false);
        }
        else
        {
            _serverButtons[0].gameObject.SetActive(false);
            _joinInputField.gameObject.SetActive(true);
            IsTryingToJoin = true;
        }
    }
}
