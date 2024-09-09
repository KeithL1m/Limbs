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

    private bool _isOnline = false;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _isOnline = ServiceLocator.Get<GameManager>().IsOnline;
    }

    private void OnEnable()
    {
        if (ServiceLocator.Get<GameManager>().IsOnline)
        {
            _headNetworkIndex = new NetworkVariable<int>(0);
            _headNetworkIndex.OnValueChanged += OnHeadIndexChanged;

            _bodyNetworkIndex = new NetworkVariable<int>(0);
            _bodyNetworkIndex.OnValueChanged += OnBodyIndexChanged;
            ChangeCurrentHead(_headNetworkIndex.Value);
            ChangeCurrentBody(_bodyNetworkIndex.Value);
        }
    }

    private void OnDisable()
    {
        if (ServiceLocator.Get<GameManager>().IsOnline)
        {
            _headNetworkIndex.OnValueChanged -= OnHeadIndexChanged;

            _bodyNetworkIndex.OnValueChanged -= OnBodyIndexChanged;
        }
    }

    public void SetPlayerIndex(int playerNum, int playerIndex)
    {
        _playerIndex = playerIndex;
        string name = "Player " + (playerNum + 1).ToString();
        _titleText.SetText(name);
        _configManager.SetPlayerName(playerIndex, name);
    }

    public void ReadyPlayer()
    {
        if (_isOnline && IsOwner)
        {
            ChangeReadyButtonSpriteClientRpc();
        }

        _configManager.SetPlayerHead(_playerIndex, _playerHead[_headIndex]);
        _configManager.SetPlayerBody(_playerIndex, _playerBody[_bodyIndex]);
        _readyButtonImage.sprite = _readySprite;
        _readyButton.enabled = false;

        _audioManager.PlaySound(_readySound, transform.position, SoundType.SFX);


        _configManager.ReadyPlayer(_playerIndex);
    }

    public void ChangeCurrentHead(int amount)
    {
        _headIndex += amount;
        if (_headIndex >= _playerHead.Count)
        {
            _headIndex = 0;
        }
        else if (_headIndex < 0)
        {
            _headIndex = _playerHead.Count - 1;
        }

        _currentHead.sprite = _playerHead[_headIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);

        if (_isOnline && IsOwner)
        {
            ChangeHeadServerRpc(_headIndex);
            return;
        }
    }

    public void ChangeCurrentBody(int amount)
    {
        _bodyIndex += amount;
        if (_bodyIndex >= _playerBody.Count)
        {
            _bodyIndex = 0;
        }
        else if (_bodyIndex < 0)
        {
            _bodyIndex = _playerBody.Count - 1;
        }

        _currentBody.sprite = _playerBody[_bodyIndex];
        _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);

        if (_isOnline && IsOwner)
        {
            ChangeBodyServerRpc(_bodyIndex);
            return;
        }
    }

    private void OnHeadIndexChanged(int oldValue, int newValue)
    {
        if (!IsOwner)
        {
            _currentHead.sprite = _playerHead[newValue];
            _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
        }
    }

    private void OnBodyIndexChanged(int oldValue, int newValue)
    {
        if (!IsOwner)
        {
            _currentBody.sprite = _playerBody[newValue];
            _audioManager.PlaySound(_selectSound, transform.position, SoundType.SFX);
        }
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

    [ClientRpc()]
    private void ChangeReadyButtonSpriteClientRpc()
    {
        if (_readyButton.targetGraphic is Image image)
        {
            _readyButton.image.sprite = image.sprite;
        }
    }
}
