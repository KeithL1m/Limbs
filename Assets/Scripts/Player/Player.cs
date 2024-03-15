using System;
using System.Collections;
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
    [HideInInspector]
    public PlayerInputHandler _inputHandler;
    private PlayerConfiguration _config;

    [SerializeField] private SpriteRenderer _playerHead;
    [SerializeField] private SpriteRenderer _playerBody;
    [SerializeField] private SpriteRenderer _playerNum;


    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] public Transform GroundCheckTransform;
    [SerializeField] private Transform attackPointTransform;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private ParticleSystem _impactParticles;

    //for melee
    [SerializeField] private Animator _animator;

    //facing left = -1, right = 1
    public int direction;
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

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _playerLimbs = GetComponent<PlayerLimbs>();
        _inputHandler = GetComponent<PlayerInputHandler>();

        _inputHandler.MeleeAttack += OnMeleeAttack;
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
            direction = 1;
        }
        else
        {
            direction = -1;
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
                _playerHead.flipX = false;
                _playerBody.flipX = false;
            }
            else
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, 0);
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

        if (!_wasOnGround && _groundCheck.isGrounded && _previousVelocity2.y < -5.0f)
        {
            _impactParticles.Play();
        }

        Debug.Log($"Check left is {checkAnimLeft.ToString()}");

        _wasOnGround = _groundCheck.isGrounded;
        _previousVelocity2 = _previousVelocity1;
        _previousVelocity1 = _rb.velocity;
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
    private bool isWinRound;
    [SerializeField] private GameObject _crownGameObject;
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

}
