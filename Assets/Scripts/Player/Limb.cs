using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Limb : MonoBehaviour
{
    public enum LimbType
    {
        Arm, 
        Leg
    }

    public enum LimbState
    {
        Attached,
        Throwing,
        Returning,
        PickUp
    }

    [HideInInspector]
    public Player _attachedPlayer;
    [HideInInspector]
    public Transform _anchorPoint = null;
    Rigidbody2D _rb;

    [HideInInspector]
    public LimbType _limbType; //this will help most with animations
    [HideInInspector]
    public LimbState _limbState;

    [SerializeField]
    private LimbData _limbData;

    //limb properties
    public float _size;
    private Vector2 _throwVelocity; //used when not aiming
    private float _throwSpeed; //used when aiming
    private float _angularVelocity;
    private float _damage;
    private float _specialDamage;

    private Vector3 _returnVelocity;

    private void Start()
    {
        _limbState = LimbState.PickUp;
        _rb = GetComponent<Rigidbody2D>();
        _rb.SetRotation(0);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        _size = _limbData._limbSize;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _angularVelocity = _limbData._angularVelocity;
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
    }

    public void ThrowLimb(int direction)
    {
        _attachedPlayer.MoveBodyDown();
        _rb.simulated = true;
        _limbState = LimbState.Throwing;

        if (_attachedPlayer._aim.x == 0.0f && _attachedPlayer._aim.y == 0.0f)
        {
            _throwVelocity.x = Mathf.Abs(_throwVelocity.x);
            _throwVelocity.x *= direction;
            _rb.velocity = _throwVelocity;
        }
        else
        {
            Vector2 tVelocity = _attachedPlayer._aim;
            tVelocity *= _throwSpeed;
            _rb.velocity = tVelocity;
        }
        _rb.angularVelocity = _angularVelocity;
    }

    private void ReturnLimb()
    {
        _limbState = LimbState.Returning;
        _throwVelocity.x *= -1;
        _rb.velocity = _returnVelocity;
    }

    public void LimbAttack()
    {
        //for if we ever do melee
    }

    // Limb damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (_limbState != LimbState.Throwing)
            return;

        PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
        _healthPlayer.AddDamage(_damage + _specialDamage);
        ReturnLimb();
    }

    // Limb pickup
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (_limbState == LimbState.Throwing || _limbState == LimbState.Attached)
            return;
        else if (_limbState == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
            return;

        if (collision.gameObject.GetComponent<Player>().CanPickUpLimb(this))
        {
            _attachedPlayer = collision.gameObject.GetComponent<Player>();
            if (_limbType == LimbType.Arm)
            {

                _rb.SetRotation(90);
            }
            if (_limbType == LimbType.Leg)
            {
                _rb.SetRotation(0);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (_limbState == LimbState.Attached)
            return;
        else if (_limbState == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
            return;

        if (_limbState == LimbState.Throwing)
        {
            _returnVelocity = -_rb.velocity;
            return;
        }

        if (collision.gameObject.GetComponent<Player>().CanPickUpLimb(this))
        {
            _attachedPlayer = collision.gameObject.GetComponent<Player>();
            if (_limbType == LimbType.Arm)
            {

                _rb.SetRotation(90);
            }
            if (_limbType == LimbType.Leg)
            {
                _rb.SetRotation(0);
            }
        }
    }
}
