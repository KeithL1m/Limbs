using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : Limb
{
    [SerializeField] private GameObject _bullet;
    public int _shotsLeft = 3;

    protected override void Initialize()
    {
        base.Initialize();

        TripleShot = true;
    }

    public override void ThrowLimb(int direction)
    {
        if (_shotsLeft > 0)
        {
            Vector2 throwDirection = new Vector2();

            if (_attachedPlayer._inputHandler.Aim.x == 0.0f && _attachedPlayer._inputHandler.Aim.y == 0.0f && !_attachedPlayer._inputHandler.FlickAiming)
            {
                throwDirection = new Vector2(direction, 0);
                
            }
            else if (_attachedPlayer._inputHandler.FlickAiming)
            {
                throwDirection = _attachedPlayer.LastAimed;
            }
            else
            {
                throwDirection = _attachedPlayer._inputHandler.Aim;
            }

            Bullet bullet = Instantiate(_bullet).GetComponent<Bullet>();
            bullet.Initialize(throwDirection, transform.position);

            Physics2D.IgnoreCollision(_attachedPlayer.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>(), true);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>(), true);
            _shotsLeft--;

            return;
        }

        TripleShot = false;
        _attachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
        State = LimbState.Throwing;

        Trail.SetActive(true);
        transform.parent = null;

        if (_attachedPlayer._inputHandler.Aim.x == 0.0f && _attachedPlayer._inputHandler.Aim.y == 0.0f && !_attachedPlayer._inputHandler.FlickAiming)
        {
            _throwVelocity.x = Mathf.Abs(_throwVelocity.x);
            _throwVelocity.x *= direction;
            LimbRB.velocity = _throwVelocity;
        }
        else if (_attachedPlayer._inputHandler.FlickAiming)
        {
            Vector2 tVelocity = _attachedPlayer.LastAimed;
            tVelocity *= _throwSpeed;
            LimbRB.velocity = tVelocity;
        }
        else
        {
            Vector2 tVelocity = _attachedPlayer._inputHandler.Aim;
            tVelocity *= _throwSpeed;
            LimbRB.velocity = tVelocity;
        }

        _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier * 0.6f, -LimbRB.velocity.y * _rVMultiplier * 0.6f, 0f);
    }
}

