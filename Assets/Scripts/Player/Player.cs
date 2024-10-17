using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerLimbs))]
public class Player : MonoBehaviour
{
    private const string HeadButtAnimName = "HeadButt";
    private const string HeadButtAnimNameLeft = "HeadButtL";
    private bool checkAnimLeft = false;

    private GameManager _gameManager;
    public enum MovementState
    {
        Move,
        Jump
    };

    public MovementState _movementState;

    //Player components
    private PlayerMovement _playerMovement;
    public PlayerMovement PlayerMovement { get { return _playerMovement; } }
    private PlayerJump _playerJump;
    private PlayerLimbs _playerLimbs;
    private AimHandler _aimHandler;
    [HideInInspector]
    public PlayerInputHandler _inputHandler;
    private PlayerConfiguration _config;
    private PlayerOnlineHelper _onlineHelper;

    [SerializeField] private SpriteRenderer _playerHead;
    [SerializeField] private SpriteRenderer _playerBody;
    [SerializeField] private SpriteRenderer _playerNum;


    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _aimTransform;


    public Transform GroundCheckTransform;
    [SerializeField] private Transform attackPointTransform;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private Material _sicknessMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private GameObject _drunkRisingBubbleParticles;
    //for melee
    [SerializeField] private Animator _animator;
    private const float _meleeCooldown = 0.8f;
    float lastMelee;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> _throwSounds;
    [SerializeField] private AudioClip _landSound;
    private AudioManager _audioManager;

    private bool _initialized = false;
    private bool _canThrow = true;
    private bool _canSwitch = true;
    private bool _wasOnGround = false;
    public Vector2 LastAimed { get; private set; } = Vector2.zero;
    private Vector2 _previousVelocity1 = Vector2.zero;
    private Vector2 _previousVelocity2 = Vector2.zero;

    private bool _canFly = false;
    public bool CanFly { get { return _canFly; } }

    public int Id { get => _id; }
    private int _id;

    private bool _isOnline = false;

    private bool isWinRound;
    [SerializeField] private GameObject _crownGameObject;

    public Action OnLanded;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _playerLimbs = GetComponent<PlayerLimbs>();
        _inputHandler = GetComponent<PlayerInputHandler>();

        _inputHandler.MeleeAttack += OnMeleeAttack;
    }

    public void Initialize(PlayerConfiguration pc)
    {
        Debug.Log($"<color=yellow>Initializing Player {pc.PlayerIndex}<color>");
        _config = pc;
        _id = pc.PlayerIndex;
        _inputHandler.InitializePlayer(_config);
        _playerLimbs.Initialize();
        _aimHandler = new AimHandler(_inputHandler, _playerMovement);

        _playerHead.sprite = _config.Head;
        _playerBody.sprite = _config.Body;
        _playerNum.sprite = _config.Num;

        _gameManager = ServiceLocator.Get<GameManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        if (_gameManager.IsOnline)
        {
            _onlineHelper = GetComponent<PlayerOnlineHelper>();
            _isOnline = true;
        }

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
        {
            return;
        }
        if (PauseManager.paused)
        {
            return;
        }
        if (_gameManager.VictoryScreen)
        {
            return;
        }

        _aimHandler.SetLastAimed();
        _aimHandler.SetDirection();

        if ((_inputHandler.LimbSwitch > 0.5f || _inputHandler.LimbSwitch < -0.5f) && _canSwitch)
        {
            _playerLimbs.SwitchLimb(_inputHandler.LimbSwitch);
            _canSwitch = false;
        }
        else if (_inputHandler.LimbSwitch < 0.5f && _inputHandler.LimbSwitch > -0.5f)
        {
            _canSwitch = true;
        }

        /*throwing limbs*/
        if (_inputHandler.ThrowLimb > 0.5f && _playerLimbs.CanThrowLimb() && _canThrow)
        {
            _audioManager.PlayRandomSound(_throwSounds.ToArray(), transform.position, SoundType.SFX);

            _playerLimbs.ThrowLimb(_aimHandler.Direction);
            _canThrow = false;
        }
        else if (_inputHandler.ThrowLimb < 0.5f)
        {
            _canThrow = true;
        }

        /*horizontal movement*/

        _playerLimbs.CheckLimbState();

        _playerMovement.Move(_playerLimbs._limbState);

        /*vertical movement*/
        _playerJump.Jump();

        //reset limb throw
        if (_inputHandler.ThrowLimb == 0.0f)
        {
            _playerLimbs._canThrow = true;
        }

        //update melee point
        if (_aimHandler.Direction == 1) // right
        {
            attackPointTransform.localPosition = new Vector3(0.56f, 0.38f, -0.6805403f);
            checkAnimLeft = false;

        }
        else if (_aimHandler.Direction == -1) // left
        {
            attackPointTransform.localPosition = new Vector3(-0.56f, 0.38f, -0.6805403f);
            checkAnimLeft = true;
        }

        //updating arrow
        (Vector3, bool) aimResults = _aimHandler.GetAimResults();
        _aimTransform.eulerAngles = aimResults.Item1;

        bool flip = aimResults.Item2;
        //Send this data to other players

        if (_isOnline)
        {
            _onlineHelper.HelpFlipBody(flip);
        }
        else
        {
            _playerHead.flipX = flip;
            _playerBody.flipX = flip;
        }
        

        if (!_wasOnGround && _groundCheck.isGrounded && _previousVelocity2.y < -5.0f)
        {
            OnLanded?.Invoke();
            _audioManager.PlaySound(_landSound, transform.position, SoundType.SFX, 0.7f);
            _impactParticles.Play();
        }

        _wasOnGround = _groundCheck.isGrounded;
        _previousVelocity2 = _previousVelocity1;
        _previousVelocity1 = _rb.velocity;
    }

    private IEnumerator MeleeDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Melee Damage Applied");
        _playerLimbs.Melee(_id);
    }

    // melee attack
    private void OnMeleeAttack(float variable)
    {
        if (Time.time - lastMelee < _meleeCooldown)
        {
            return;
        }
        lastMelee = Time.time;
        Debug.Log("Melee Anim Triggered");
        if (checkAnimLeft)
        {
            _animator.SetTrigger(HeadButtAnimNameLeft);
        }
        else
        {
            _animator.SetTrigger(HeadButtAnimName);
        }

        float animLength = GetAnimLength(HeadButtAnimName) * 0.5f;
        StartCoroutine(MeleeDelay(animLength));
    }

    private float GetAnimLength(string animName)
    {
        // Getting the animation length HASH CODE
        foreach (var anim in _animator.runtimeAnimatorController.animationClips)
        {
            if (string.CompareOrdinal(anim.name, animName) == 0)
            {
                return anim.length;
            }
        }

        Debug.LogError($"Animatin Clip Not Found: {animName}");
        return 0f;
    }

    public void SetCanFly(bool isCan)
    {
        _canFly = isCan;
    }

    public void AddScore()
    {
        _config.Score++;
        SetDisplayCrown(true);
    }

    public int GetScore()
    {
        return _config.Score;
    }

    public string GetName()
    {
        return _config.Name;
    }

    public void SetDisplayCrown(bool value)
    {
        if (isWinRound)
        {
            if (!value)
            {
                isWinRound = false;
                _crownGameObject.SetActive(false);
                _playerNum.transform.localPosition -= transform.up.normalized * 1.2f;

            }
        }
        else
        {
            if (value)
            {

                isWinRound = true;
                _crownGameObject.SetActive(true);
                _playerNum.transform.localPosition += transform.up.normalized * 1.2f;

            }
        }
    }

    public void Death()
    {
        SetDisplayCrown(false);
    }

    public SpriteRenderer GetArrow()
    {
        return _aimTransform.GetComponentInChildren<SpriteRenderer>();
    }

    public Vector3 GetSize()
    {
        return _playerLimbs.GetSize();
    }

    public void ZeroVelocity()
    {
        _playerMovement.ZeroVelocity();
    }

    public void MakeAimOpposite()
    {
        _aimHandler.MakeAimOpposite();
        _playerHead.material = _sicknessMaterial;
        _drunkRisingBubbleParticles.SetActive(true);
    }

    public void MakeAimNormal()
    {
        _aimHandler.MakeAimNormal();
        _playerHead.material = _defaultMaterial;
        _drunkRisingBubbleParticles.SetActive(false);
    }

    public float GetRBGravity()
    {
        return _rb.gravityScale;
    }

    public void FlipSprite(bool value)
    {
        _playerBody.flipX = value;
        _playerHead.flipY = value;
    }
}
