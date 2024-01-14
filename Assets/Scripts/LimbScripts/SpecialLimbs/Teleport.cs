using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Limb
{
    private bool _teleport = false;
    private float _timer = 0f;

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        _teleport = true;
    }

    private void Update()
    {
        if (_teleport)
        {
            _timer += Time.deltaTime;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (_timer < 0.05f)
            return;
        if (State != LimbState.Throwing)
            return;

        //set teleport position and add delay?

        _attachedPlayer.transform.position = transform.position;
        ServiceLocator.Get<LimbManager>().RemoveLimb(this);
        Destroy(gameObject);
    }
}

