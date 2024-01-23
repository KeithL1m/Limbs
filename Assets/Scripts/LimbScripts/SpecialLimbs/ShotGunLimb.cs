using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunLimb : Limb
{
    [Header("Special Info")]
    [SerializeField, Range(0, 100)] float _forceAmt = 0;

    public override void ThrowLimb(int direction)
    {
        _attachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
        State = LimbState.Throwing;

        Trail.SetActive(true);

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
        _attachedPlayer.GetComponent<Rigidbody2D>().AddForce(LimbRB.velocity.normalized * (-_forceAmt), ForceMode2D.Impulse);

        _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier * 0.6f, -LimbRB.velocity.y * _rVMultiplier * 0.6f, 0f);
    }
}
