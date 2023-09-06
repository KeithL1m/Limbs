using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    Player _player;
    PlayerJump _playerJump;

    float _moveInput;

    [Header("Customizable")]
    [SerializeField] float _2LegMoveSpeed;
    [SerializeField] float _1LegMoveSpeed;
    [SerializeField] float _noLegSpeed;
    [SerializeField] float _hopForce;
    private float _hopTimer = 0.0f;
    [SerializeField] float _maxHopTime;

    [SerializeField] float _startMovePoint = 0.5f;
    [SerializeField] float _smoothMoveSpeed = 0.06f; //the higher the number the less responsive it gets

    Vector3 zeroVector = Vector3.zero;

    public bool facingRight;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _playerJump = GetComponent<PlayerJump>();
    }

    public void Move(Player.LimbState state)
    {
        float moveSpeed = 0f;
        if (_moveInput <= -_startMovePoint)
        {
            moveSpeed = -1f;
            facingRight = false;
        }
        else if (_moveInput >= _startMovePoint)
        {
            moveSpeed = 1f;
            facingRight = true;
        }

        switch (state)
        {
            case Player.LimbState.TwoLeg:
                moveSpeed *= _2LegMoveSpeed;
                break;
            case Player.LimbState.OneLeg:
                moveSpeed *= _1LegMoveSpeed;
                break;
            case Player.LimbState.NoLimb:
                _hopTimer -= Time.deltaTime;
                Hop(moveSpeed);
                moveSpeed *= _noLegSpeed;
                break; 
            default: break;
        }

        Vector3 targetVelocity = new Vector2(moveSpeed, _rb.velocity.y);
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref zeroVector, _smoothMoveSpeed);
    }

    private void Hop(float moveSpeed)
    {
        if (_playerJump.IsGrounded() && _hopTimer <= 0.0f)
        {
            _hopTimer = _maxHopTime;
            _rb.AddForce(_rb.mass * Vector2.up * _hopForce * Mathf.Abs(moveSpeed), ForceMode2D.Impulse);
        }
    }

    public void MoveInput(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<float>();
}
