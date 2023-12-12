using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupController : MonoBehaviour
{
    private GameLoader _loader = null;

    private int _playerIndex;

    private ConfigurationManager _configManager;

    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private GameObject _readyPanel;
    [SerializeField]
    private Button _name;
    [SerializeField]
    private List<Sprite> _playerHead;
    [SerializeField]
    private List<Sprite> _playerBody;
    [SerializeField]
    private Image _currentHead;
    [SerializeField]
    private Image _currentBody;
    [SerializeField]
    private Image _readyButton;
    [SerializeField]
    private Sprite _readySprite;

    private int _headIndex;
    private int _bodyIndex;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
    }

    public void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        string name = "Player " + (pi + 1).ToString();
        _titleText.SetText(name);
        _configManager.SetPlayerName(_playerIndex, name);
    }

    public void ReadyPlayer()
    {
        _configManager.ReadyPlayer(_playerIndex);
        _configManager.SetPlayerHead(_playerIndex, _playerHead[_headIndex]);
        _configManager.SetPlayerBody(_playerIndex, _playerBody[_bodyIndex]);
        _readyButton.sprite = _readySprite;
    }

    public void ChangeCurrentHeadLeft()
    {
        if (_headIndex == 0)
        {
            _headIndex = _playerHead.Count - 1;
        }
        else
        {
            _headIndex--;
        }

        _currentHead.sprite = _playerHead[_headIndex];
    }

    public void ChangeCurrentHeadRight()
    {
        if (_headIndex == _playerHead.Count - 1)
        {
            _headIndex = 0;
        }
        else
        {
            _headIndex++;
        }

        _currentHead.sprite = _playerHead[_headIndex];
    }

    public void ChangeCurrentBodyLeft()
    {
        if (_bodyIndex == 0)
        {
            _bodyIndex = _playerBody.Count - 1;
        }
        else
        {
            _bodyIndex--;
        }

        _currentBody.sprite = _playerBody[_bodyIndex];
    }

    public void ChangeCurrentBodyRight()
    {
        if (_bodyIndex == _playerBody.Count - 1)
        {
            _bodyIndex = 0;
        }
        else
        {
            _bodyIndex++;
        }

        _currentBody.sprite = _playerBody[_bodyIndex];
    }

}
