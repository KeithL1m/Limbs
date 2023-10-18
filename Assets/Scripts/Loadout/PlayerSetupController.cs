using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupController : MonoBehaviour
{
    private int _playerIndex;

    private ConfigurationManager _configManager;

    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private GameObject _readyPanel;
    [SerializeField]
    private Button _name;

    private void Awake()
    {
        _configManager = FindObjectOfType<ConfigurationManager>();
    }

    public void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        string name = "Player " + (pi + 1).ToString();
        _titleText.SetText(name);
        _configManager.SetPlayerName(_playerIndex, name);
    }

    public void SetHead(Sprite head)
    {
        _configManager.SetPlayerHead(_playerIndex, head);
    }

    public void SetBody(Sprite body)
    {
        _configManager.SetPlayerBody(_playerIndex, body);
    }

    public void ReadyPlayer()
    {
        _configManager.ReadyPlayer(_playerIndex);
    }
}
