using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerLimbs _playerLimbs;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private Player _player;
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _headRotation;

    [Header("Variables")]
    [SerializeField] private float _maxStretchY;
    [SerializeField] private float _maxStretchX;

    [SerializeField] private float _maxRotation;
    [SerializeField] private float _maxFallSpeed;

    [SerializeField] private float _maxSquashTime;
    private float _squashTimer;
    [SerializeField] private float _maxSquashX;
    [SerializeField] private float _maxSquashY;

    private bool _onLanded;
    private bool _isRestoring = false;
    private bool _isSquashing = false;
    private float _maxSpeed;

    private float _rotationSpeed;

    private void Awake()
    {
        ServiceLocator.Get<GameLoader>().CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _playerMovement.OnMove += AnimateBody;
        _playerMovement.OnMove += AnimateHead;
        _player.OnLanded += StartSquash;
        _playerJump.OnStartJump += StartJumpAnim;
        _maxSpeed = _playerMovement.GetMaxSpeed();
    }

    private void AnimateHead()
    {
        float speed = _rb.velocity.x;

        float currentRotation = (speed / _maxSpeed) * _maxRotation;

        float smoothRotate = Mathf.SmoothDamp(_headRotation.rotation.z, currentRotation, ref _rotationSpeed, 0.001f);

        Quaternion rotation = Quaternion.Euler(0f, 0f, smoothRotate);
        _headRotation.rotation = rotation;
    }

    private void AnimateBody()
    {
        if (_onLanded)
        {
            Debug.Log("Start Squash");
            _squashTimer += Time.deltaTime;

            if (_squashTimer >= _maxSquashTime)
            {
                _onLanded = false;
            }

            if (_isSquashing)
            {
                if (_squashTimer < _maxSquashTime * 0.5f)
                {
                    float t = _squashTimer / (_maxSquashTime * 0.5f);
                    float squashX = Mathf.Lerp(1f, _maxSquashX, t);
                    float squashY = Mathf.Lerp(1f, _maxSquashY, t);

                    _body.transform.localScale = new Vector3(squashX, squashY, 1f);
                }
                else if (!_isRestoring)
                {
                    _squashTimer = 0f;
                    _isRestoring = true;
                    _isSquashing = false;
                }
            }

            if (_isRestoring)
            {
                if (_squashTimer < _maxSquashTime * 0.5f)
                {
                    float t = _squashTimer / (_maxSquashTime * 0.5f);
                    float squashX = Mathf.Lerp(_maxSquashX, 1f, t);
                    float squashY = Mathf.Lerp(_maxSquashY, 1f, t);

                    _body.transform.localScale = new Vector3(squashX, squashY, 1f);
                }
                else
                {
                    _onLanded = false;
                    _isRestoring = false;
                }
            }
        }
        else
        {
            float velocityY = _rb.velocity.y / _maxFallSpeed;
            float stretchX = Mathf.Lerp(1f, _maxStretchX, velocityY);
            float stretchY = Mathf.Lerp(1f, _maxStretchY, velocityY);

            _body.transform.localScale = new Vector3(stretchX, stretchY, 1f);
        }

        if (_body.localScale == Vector3.one)
        {
            foreach (var limb in _playerLimbs._limbs)
            {
                if (!limb)
                {
                    continue;
                }

                if(limb.transform.localScale.x != 1 || limb.transform.localScale.y != 1)
                {
                    limb.transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void StartSquash()
    {
        _isSquashing = true;
        _onLanded = true;
        _squashTimer = 0.0f;
    }

    private void StartJumpAnim()
    {
        _onLanded = false;
    }
}
