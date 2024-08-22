using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerJump _playerJump;
    private PlayerInputHandler _inputHandler;
    private PlayerHealth _pHealth;

    [SerializeField] private FootstepController _footsteps;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private ParticleSystem _walkDust;
    private ParticleSystem.EmissionModule _walkDustEmission;
    [SerializeField] private ParticleSystem _speedUpDust;
    private ParticleSystem.EmissionModule _speedUpEmission;
    private AudioManager _audioManager;

    [SerializeField] private Animator anchorsAnim;

    [Header("Customizable")]
    [SerializeField] private float[] extraAcceleration;
    private int currentAccelerationLimbNumber = 0;

    [SerializeField] private float _2LegMoveSpeed;
    [SerializeField] private float _1LegMoveSpeed;
    [SerializeField] private float _noLegSpeed;
    [SerializeField] private float _hopForce;
    private float _hopTimer = 0.0f;
    [SerializeField] private float _maxHopTime;
    [SerializeField] private float _startMovePoint = 0.5f;
    [SerializeField] private float _smoothMoveSpeed = 0.06f; //the higher the number the less responsive it get

    [SerializeField] private List<AudioClip> _hopSound;

    Vector3 zeroVector = Vector3.zero;

    public bool facingRight;
    private bool _dust;

    public Action OnMove;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerJump = GetComponent<PlayerJump>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _pHealth = GetComponent<PlayerHealth>();
        _walkDustEmission = _walkDust.emission;
        _speedUpEmission = _speedUpDust.emission;
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    public void Move(PlayerLimbs.LimbState state)
    {
        float moveSpeed = 0f;
        if (_inputHandler.Movement <= -_startMovePoint)
        {
            moveSpeed = -1f;
            facingRight = false;
        }
        else if (_inputHandler.Movement >= _startMovePoint)
        {
            moveSpeed = 1f;
            facingRight = true;
        }

        switch (state)
        {
            case PlayerLimbs.LimbState.TwoLeg:
                moveSpeed *= _2LegMoveSpeed * (1 + (extraAcceleration[currentAccelerationLimbNumber]));
                break;
            case PlayerLimbs.LimbState.OneLeg:
                moveSpeed *= _1LegMoveSpeed * (1 + (extraAcceleration[currentAccelerationLimbNumber]));
                break;
            case PlayerLimbs.LimbState.NoLimb:
                _hopTimer -= Time.deltaTime;
                if (_hopTimer <= 0.0f && Mathf.Abs(moveSpeed) > 0.1f)
                {
                    Hop(moveSpeed);
                }
                moveSpeed *= _noLegSpeed;
                break;
            default: break;
        }

        anchorsAnim.SetFloat("speed", moveSpeed);

        if (_playerJump.IsGrounded() && _rb.velocity.magnitude > 1 && state != PlayerLimbs.LimbState.NoLimb)
        {
            _footsteps.StartWalking();
        }
        else
        {
            _footsteps.StopWalking();
        }

        Vector3 targetVelocity = new Vector2(moveSpeed, _rb.velocity.y);
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref zeroVector, _smoothMoveSpeed);

        OnMove?.Invoke();

        if (_rb.velocity.magnitude > 4.0f)
        {
            _dust = true;
        }
        else
        {
            _dust = false;
        }

        if (_dust && _groundCheck.isGrounded && !_pHealth.IsDead())
        {
            _walkDustEmission.rateOverTime = 50;
            _speedUpEmission.rateOverTime = 50;
        }
        else
        {
            _speedUpEmission.rateOverTime = 0;
            _walkDustEmission.rateOverTime = 0;
        }
    }

    private void Hop(float moveSpeed)
    {
        if (_playerJump.IsGrounded())
        {
            _audioManager.PlayRandomSound(_hopSound.ToArray(), transform.position, SoundType.SFX);
            _hopTimer = _maxHopTime;
            _rb.AddForce(_rb.mass * Vector2.up * _hopForce * Mathf.Abs(moveSpeed), ForceMode2D.Impulse);
        }
    }

    public void AddAccelerationLimb()
    {
        currentAccelerationLimbNumber++;
        _speedUpDust.Play();
    }

    public void RemoveAccelerationLimb()
    {
        currentAccelerationLimbNumber--;
        if (currentAccelerationLimbNumber <= 0)
        {
            currentAccelerationLimbNumber = 0;
            _speedUpDust.Stop();
        }
    }

    public void ZeroVelocity()
    {
        _rb.velocity = Vector3.zero;
    }

    public float GetMaxSpeed()
    {
        return _2LegMoveSpeed;
    }
}
