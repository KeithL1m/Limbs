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

    private void Start()
    {
        _configManager = FindObjectOfType<ConfigurationManager>();
    }

    public void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        _titleText.SetText("Player " + (pi + 1).ToString());
    }

    public void SetHead(Sprite head)
    {
        _configManager.SetPlayerHead(_playerIndex, head);
    }    

    public void ReadyPlayer()
    {
        _configManager.ReadyPlayer(_playerIndex);
    }
}
