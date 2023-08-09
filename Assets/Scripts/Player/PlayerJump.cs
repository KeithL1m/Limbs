using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] GroundCheck _groundCheck;
    float _jumpInput;
    Rigidbody2D _rb;
    Player _player;

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
    private float _maxJumpBufferFrames;
    [SerializeField]
    private float _maxCoyoteFrames;

    private float _gravityScaleFactor;
    private float _jumpGravity;
    private float _initJumpSpeed;
    private float _jumpBufferFrames;
    private float _coyoteFrames;
    
    bool _canJump;
    public bool _canDoubleJump;
    public bool _isDoubleJumping;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();

        _jumpGravity = -2 * _jumpHeight / Mathf.Pow(_jumpTime, 2);
        _gravityScaleFactor = _jumpGravity / Physics2D.gravity.y;
        _rb.gravityScale = _gravityScaleFactor;
        _initJumpSpeed = -_jumpGravity * _jumpTime;
    }

    public void Jump()
    {
        if (IsGrounded() && _player._movementState == Player.MovementState.Move)
        {
            _coyoteFrames = _maxCoyoteFrames;
            _canDoubleJump = false;
            _isDoubleJumping = false;
        }
        else
        {
            _coyoteFrames--;
            if (!_isDoubleJumping)
            {
                _canDoubleJump = true;
            }
        }

        if (_jumpInput > 0.5f && _canJump)
        {
            _jumpBufferFrames = _maxJumpBufferFrames;
        }
        else
        {
            _jumpBufferFrames--;
        }

        if (_jumpBufferFrames > 0f && _coyoteFrames > 0f || _canDoubleJump && _jumpInput > 0.5f && _canJump) 
        {
            _jumpBufferFrames = 0f;
            _coyoteFrames = 0f;
            StartJump();
        }
        else if (_player._movementState == Player.MovementState.Jump)
        {
            JumpUpdate();
        }

        if (_jumpInput < 0.5f) 
        {
            _canJump = true;
        }
    } 

    private void StartJump()
    {
        _canJump = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, 0f);
        _rb.gravityScale = _gravityScaleFactor;
        _player._movementState = Player.MovementState.Jump;

        if (_canDoubleJump)
        {
            _rb.AddForce(_rb.mass * Vector2.up * _initJumpSpeed * 0.6f, ForceMode2D.Impulse);
            _canDoubleJump = false;
            _isDoubleJumping = true;
        }
        else
        {
            _rb.AddForce(_rb.mass * Vector2.up * _initJumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void JumpUpdate()
    {
        if (_jumpInput == 0f && _rb.velocity.y > 0f)
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

    public void JumpInput(InputAction.CallbackContext ctx) => _jumpInput = ctx.action.ReadValue<float>();
}
