using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private GroundCheck _groundCheck;
    
    private Rigidbody2D _rb;
    private Player _player;
    private PlayerInputHandler _inputhandler;

    [Header("Customizable")]
    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private float _jumpTime;
    [SerializeField]
    private float _fallFasterGravityFactor;
    [SerializeField]
    private float _earlyExitGravityFactor;
    [SerializeField]
    private float _maxJumpBufferTime;
    [SerializeField]
    private float _maxCoyoteTime;
    [SerializeField]
    private ParticleSystem _jumpParticles;
    [SerializeField]
    private ParticleSystem _dJumpParticles;

    private float flyPower;

    private float _gravityScaleFactor;
    private float _jumpGravity;
    private float _initJumpSpeed;
    private float _jumpBufferTime;
    private float _coyoteTime;
    
    private bool _canJump;
    private bool _canDoubleJump;
    private bool _isDoubleJumping;

    public Action OnStartJump;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _inputhandler = GetComponent<PlayerInputHandler>();

        _jumpGravity = -2 * _jumpHeight / Mathf.Pow(_jumpTime, 2);
        _gravityScaleFactor = _jumpGravity / Physics2D.gravity.y;
        _rb.gravityScale = _gravityScaleFactor;
        _initJumpSpeed = -_jumpGravity * _jumpTime;
    }

    public void Jump()
    {
        //if (GetComponent<PlayerHealth>().IsDead())
        //    return;
        NormalJump();
    }

    private void NormalJump()
    {
        if (IsGrounded() && _player._movementState == Player.MovementState.Move)
        {
            _coyoteTime = _maxCoyoteTime;
            if (_player._movementState == Player.MovementState.Move)
            {
                _canDoubleJump = false;
                _isDoubleJumping = false;
            }
        }
        else
        {
            _coyoteTime -= Time.deltaTime;
            if (!_isDoubleJumping && _coyoteTime <= 0)
            {
                _canDoubleJump = true;
            }
        }

        if (_inputhandler.Jump > 0.5f && _canJump)
        {
            _jumpBufferTime = _maxJumpBufferTime;
        }
        else
        {
            _jumpBufferTime -= Time.deltaTime;
        }

        if (_jumpBufferTime > 0f && _coyoteTime > 0f || _canDoubleJump && _inputhandler.Jump > 0.5f && _canJump)
        {
            _jumpBufferTime = 0f;
            _coyoteTime = 0f;
            StartJump();
        }
        else if (_player._movementState == Player.MovementState.Jump)
        {
            JumpUpdate();
        }

        if (_inputhandler.Jump < 0.5f)
        {
            _canJump = true;
        }
    }


    private void StartJump()
    {
        OnStartJump?.Invoke();
        _canJump = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, 0f);
        _rb.gravityScale = _gravityScaleFactor;
        _player._movementState = Player.MovementState.Jump;

        if (_canDoubleJump)
        {
            _rb.AddForce(_rb.mass * Vector2.up * _initJumpSpeed * 0.6f, ForceMode2D.Impulse);
            _canDoubleJump = false;
            _isDoubleJumping = true;
            _dJumpParticles.Play();
        }
        else
        {
            if (!_player.CanFly)
            {
                _rb.AddForce(_rb.mass * Vector2.up * _initJumpSpeed, ForceMode2D.Impulse);
                _jumpParticles.Play();
            }
            else
            {
                _rb.AddForce(_player._inputHandler.Aim * flyPower, ForceMode2D.Impulse);
            }
        }
    }

    private void JumpUpdate()
    {
        if (_inputhandler.Jump == 0f && _rb.velocity.y > 0f)
        {
            _rb.gravityScale = _gravityScaleFactor * _earlyExitGravityFactor;
        }
        else if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _gravityScaleFactor * _fallFasterGravityFactor;
        }
        else
        {
            _rb.gravityScale = _gravityScaleFactor;
        }

        if (IsGrounded() && _rb.velocity.y <= 0.0f)
        {
            _player._movementState = Player.MovementState.Move;
            _rb.gravityScale = _gravityScaleFactor;
        }
    }

    public bool IsGrounded()
    {
        return _groundCheck.isGrounded;
    }
}
