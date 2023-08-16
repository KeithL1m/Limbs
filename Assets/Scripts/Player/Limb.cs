using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Limb : MonoBehaviour
{
    [SerializeField]
    private LimbData _limbData ;
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

    public Player _attachedPlayer;
    public Transform _anchorPoint = null;
    Rigidbody2D _rb;

    public LimbType _limbType; //this will help most with animations
    public LimbState _limbState;
    private Vector2 _throwVelocity;
    private float _angularVelocity;
    private float _damage;
    private float _specialDamage;

    private void Start()
    {
        _limbState = LimbState.PickUp;
        _rb = GetComponent<Rigidbody2D>();
                            _rb.SetRotation(0);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _angularVelocity = _limbData._angularVelocity;
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
    }

    public void ThrowLimb(int direction)
    {
        _rb.simulated = true;
        _limbState = LimbState.Throwing;
        _throwVelocity.x = Mathf.Abs(_throwVelocity.x);
        _throwVelocity.x *= direction;
        _rb.velocity = _throwVelocity;
        _rb.angularVelocity = _angularVelocity;
    }

    private void ReturnLimb()
    {
        _limbState = LimbState.Returning;
        _throwVelocity.x *= -1;
        _rb.velocity = _throwVelocity;
    }

    public void LimbAttack()
    {
        //for if we ever do melee
    }

    // Limb damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _limbState == LimbState.Throwing)
        {
            PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
            _healthPlayer.AddDamage(_damage + _specialDamage);
            ReturnLimb();

        }
    }

    // Limb knockback and pickup
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        else if (_limbState == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
        {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        else if (_limbState == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
        {
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
