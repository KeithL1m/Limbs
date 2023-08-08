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
    private float _jumpBufferLength;

    private float _gravityScaleFactor;
    private float _jumpGravity;
    private float _initJumpSpeed;
    private float _jumpBufferTimer;
    


    bool _canJump;

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
        if (_jumpInput > 0.5f)
        {
            _jumpBufferTimer = _jumpBufferLength;
        }
        else
        {
            _jumpBufferTimer -= Time.deltaTime;
        }


        if (_jumpBufferTimer > 0f && IsGrounded() && _canJump) 
        {
            _jumpBufferTimer = 0f;
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
        _rb.AddForce(_rb.mass * Vector2.up * _initJumpSpeed, ForceMode2D.Impulse);
        _player._movementState = Player.MovementState.Jump;
    }

    private void JumpUpdate()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _gravityScaleFactor * _fallFasterGravityFactor;
        }
        else
        {
            _rb.gravityScale = _gravityScaleFactor;
        }


        if (IsGrounded() && _rb.velocity.y <= 0.0f)
        {
            Debug.Log("Moving");
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
