using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditorInternal;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerLimbs))]
public class Player : MonoBehaviour
{
    public enum MovementState
    {
        Move,
        Jump
    };

    public MovementState _movementState;

    //Player components
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private PlayerLimbs _playerLimbs;
    [HideInInspector]
    public  PlayerInputHandler _inputHandler;
    private PlayerConfiguration _config;

    [SerializeField] private SpriteRenderer _playerHead;
    [SerializeField] private SpriteRenderer _playerBody;

    //
    // make all limbs get thrown from same place?
    //
    //[SerializeField] Transform _leftLaunchPoint;
    //[SerializeField] Transform _rightLaunchPoint;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] public Transform _groundCheck;

    //facing left = -1, right = 1
    public int direction;
    private bool _initialized = false;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _playerLimbs = GetComponent<PlayerLimbs>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    public void Initialize(PlayerConfiguration pc)
    {
        _config = pc;

        _inputHandler.InitializePlayer(_config);

        _playerHead.sprite = _config.Head;
        _playerBody.sprite = _config.Body;

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
            return;
        if (PauseManager.paused) 
            return;

        if (_playerMovement.facingRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        /*throwing limbs*/
        if (_inputHandler.ThrowLimb > 0.5f && _playerLimbs.CanThrowLimb()) 
        {
            _playerLimbs.ThrowLimb(direction);
        }

        //limb attack?


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

        //updating arrow
        if (_inputHandler.Aim.x == 0.0f && _inputHandler.Aim.y == 0.0f)
        {
            if (direction == 1)
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, -180);
            }
            else
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            _aimTransform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-_inputHandler.Aim.y, -_inputHandler.Aim.x) * Mathf.Rad2Deg);
        }
    }

    public void AddScore()
    {
        _config.Score++;
    }

    public int GetScore()
    {
        return _config.Score;
    }

    public string GetName()
    {
        return _config.Name;
    }

    public SpriteRenderer GetArrow()
    {
        return _aimTransform.GetComponentInChildren<SpriteRenderer>();
    }

}
