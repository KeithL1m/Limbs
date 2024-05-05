using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [HideInInspector]
    public PlayerInputHandler _inputHandler;
    private PlayerConfiguration _config;

    [SerializeField] private SpriteRenderer _playerHead;
    [SerializeField] private SpriteRenderer _playerBody;
    [SerializeField] private SpriteRenderer _playerNum;


    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _aimTransform;
    private Vector3 _mousePosition;
    private Camera _camera;


    [SerializeField] public Transform GroundCheckTransform;
    [SerializeField] private Transform attackPointTransform;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private Material _sicknessMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private GameObject _drunkRisingBubbleParticles;
    //for melee
    [SerializeField] private Animator _animator;
    private float _meleeCooldown = 0.8f;
    float lastMelee;

    //facing left = -1, right = 1
    public int direction;
    private bool _initialized = false;
    private bool _canThrow = true;
    private bool _canSwitch = true;
    private bool _wasOnGround = false;
    private bool _aimConfused = false;
    public Vector2 LastAimed { get; private set; } = Vector2.zero;
    private Vector2 _previousVelocity1 = Vector2.zero;
    private Vector2 _previousVelocity2 = Vector2.zero;

    private bool _canFly = false;
    public bool CanFly { get { return _canFly; } }

    public Action OnLanded;

    public int Id { get => _id; }
    private int _id;

    private bool isWinRound;
    [SerializeField] private GameObject _crownGameObject;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _playerLimbs = GetComponent<PlayerLimbs>();
        _inputHandler = GetComponent<PlayerInputHandler>();

        _inputHandler.MeleeAttack += OnMeleeAttack;

        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Initialize(PlayerConfiguration pc)
    {
        Debug.Log($"<color=yellow>Initializing Player {pc.PlayerIndex}<color>");
        _config = pc;
        _id = pc.PlayerIndex;
        _inputHandler.InitializePlayer(_config);
        _playerLimbs.Initialize();

        _playerHead.sprite = _config.Head;
        _playerBody.sprite = _config.Body;
        _playerNum.sprite = _config.Num;

        _gameManager = ServiceLocator.Get<GameManager>();

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
            return;
        if (PauseManager.paused)
            return;
        if (_gameManager.VictoryScreen)
            return;

        if (LastAimed != new Vector2(_inputHandler.Aim.x, _inputHandler.Aim.y) && _inputHandler.FlickAiming)
        {
            if (_inputHandler.Aim.magnitude > 0.6f)
            {
                if (_inputHandler.Aim.x != 0.0f && _inputHandler.Aim.y != 0.0f)
                {
                    LastAimed = new Vector2(_inputHandler.Aim.x, _inputHandler.Aim.y);
                }
            }
        }

        if (_playerMovement.facingRight)
        {
            if (_aimConfused)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
        }
        else
        {
            if (_aimConfused)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }

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
            _playerLimbs.ThrowLimb(direction);
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
        if (direction == 1) // right
        {
            attackPointTransform.localPosition = new Vector3(0.56f, 0.38f, -0.6805403f);
            checkAnimLeft = false;

        }
        else if (direction == -1) // left
        {
            attackPointTransform.localPosition = new Vector3(-0.56f, 0.38f, -0.6805403f);
            checkAnimLeft = true;
        }

        //updating arrow

        if (_inputHandler.Aim.x == 0.0f && _inputHandler.Aim.y == 0.0f && !_inputHandler.FlickAiming)
        {
            if (direction == 1)
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, -180);
            }
            else
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (_playerMovement.facingRight)
            {
                _playerHead.flipX = false;
                _playerBody.flipX = false;
            }
            else
            {
                _playerHead.flipX = true;
                _playerBody.flipX = true;
            }
        }
        else if (!_inputHandler.FlickAiming)
        {
            _aimTransform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-_inputHandler.Aim.y, -_inputHandler.Aim.x) * Mathf.Rad2Deg);
            if (_inputHandler.Aim.x > 0)
            {
                _playerHead.flipX = false;
                _playerBody.flipX = false;
            }
            else
            {
                _playerHead.flipX = true;
                _playerBody.flipX = true;
            }
        }
        else
        {
            _aimTransform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-LastAimed.y, -LastAimed.x) * Mathf.Rad2Deg);
            if (LastAimed.x > 0)
            {
                _playerHead.flipX = false;
                _playerBody.flipX = false;
            }
            else
            {
                _playerHead.flipX = true;
                _playerBody.flipX = true;
            }
        }

        //mouse aiming
        //Mouse mouse = Mouse.current;
        //_mousePosition = _camera.ScreenToWorldPoint(mouse.position.ReadValue()) - transform.position;
        //float angle = Mathf.Atan2(-_mousePosition.y, -_mousePosition.x) * Mathf.Rad2Deg;

        //_aimTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
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
        _aimConfused = true;
        _playerHead.material = _sicknessMaterial;
        _drunkRisingBubbleParticles.SetActive(true);
    }

    public void MakeAimNormal()
    {
        _aimConfused = false;
        _playerHead.material = _defaultMaterial;
        _drunkRisingBubbleParticles.SetActive(false);

    }
}
