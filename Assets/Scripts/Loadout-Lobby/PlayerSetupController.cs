using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupController : NetworkBehaviour
{
    private GameLoader _loader = null;

    private int _playerIndex;

    private ConfigurationManager _configManager;
    private AudioManager _audioManager;

    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private GameObject _readyPanel;
    [SerializeField]
    private Button _name;
    [SerializeField]
    private Button _readyButton;
    [SerializeField]
    private List<Sprite> _playerHead;
    [SerializeField]
    private List<Sprite> _playerBody;
    [SerializeField]
    private Image _currentHead;
    [SerializeField]
    private Image _currentBody;
    [SerializeField]
    private Image _readyButtonImage;
    [SerializeField]
    private Sprite _readySprite;
    [SerializeField]
    private AudioClip _selectSound;
    [SerializeField]
    private AudioClip _readySound;

    private int _headIndex;
    private int _bodyIndex;

    private NetworkVariable<int> _headNetworkIndex;
    private NetworkVariable<int> _bodyNetworkIndex;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    private void OnEnable()
    {
        var multiplayerHandler = ServiceLocator.Get<MultiplayerHandler>();
        if (multiplayerHandler)
        {
            _headNetworkIndex = new NetworkVariable<int>(0);
            _headNetworkIndex.OnValueChanged += OnHeadIndexChanged;

            _bodyNetworkIndex = new NetworkVariable<int>(0);
            _bodyNetworkIndex.OnValueChanged += OnBodyIndexChanged;
        }
    }

    private void OnDisable()
    {
        var multiplayerHandler = ServiceLocator.Get<MultiplayerHandler>();
        if (multiplayerHandler)
        {
            _headNetworkIndex.OnValueChanged -= OnHeadIndexChanged;

            _bodyNetworkIndex.OnValueChanged -= OnBodyIndexChanged;
        }
    }

    public void SetPlayerIndex(int playerNum, int playerIndex)
    {
        _playerIndex = playerIndex;
        string name = "Player " + playerNum.ToString();
        _titleText.SetText(name);
        _configManager.SetPlayerName(playerIndex, name);
    }

    public void ReadyPlayer()
    {
        _configManager.SetPlayerHead(_playerIndex, _playerHead[_headIndex]);
        _configManager.SetPlayerBody(_playerIndex, _playerBody[_bodyIndex]);
        _readyButtonImage.sprite = _readySprite;
        _readyButton.enabled = false;

        _audioManager.PlaySound(_readySound, transform.position, SoundType.SFX);


        _configManager.ReadyPlayer(_playerIndex);
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

        if (_headNetworkIndex != null && IsOwner)
        {
            ChangeHeadServerRpc(_headIndex);
            return;
        }

        _currentHead.sprite = _playerHead[_headIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
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

        if (_headNetworkIndex != null && IsOwner)
        {
            ChangeHeadServerRpc(_headIndex);
            return;
        }

        _currentHead.sprite = _playerHead[_headIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
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

        if (_bodyNetworkIndex != null && IsOwner)
        {
            ChangeBodyServerRpc(_bodyIndex);
            return;
        }

        _currentBody.sprite = _playerBody[_bodyIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
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

        if (_bodyNetworkIndex != null && IsOwner)
        {
            Debug.Log("ChangeBody");
            ChangeBodyServerRpc(_bodyIndex);
            return;
        }

        _currentBody.sprite = _playerBody[_bodyIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeHeadServerRpc(int value)
    {
        _headNetworkIndex.Value = value;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeBodyServerRpc(int value)
    {
        _bodyNetworkIndex.Value = value;
    }

    private void OnHeadIndexChanged(int oldValue, int newValue)
    {
        _currentHead.sprite = _playerHead[newValue];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
    }

    private void OnBodyIndexChanged(int oldValue, int newValue)
    {
        _currentBody.sprite = _playerBody[newValue];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
    }
}
