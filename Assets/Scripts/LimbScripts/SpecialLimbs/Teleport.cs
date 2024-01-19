using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Limb
{
    private bool _teleport = false;

    private Vector3 _teleportPosition = new();
    private float _timer = 0f;

    Player _teleportedPlayer;

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        ServiceLocator.Get<CameraManager>().AddPlayer(gameObject);

        _teleport = true;
        _teleportedPlayer = _attachedPlayer;
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

        bool top = false;
        bool bottom = false;

        ContactPoint2D point2D = collision.contacts[0];

        Vector2 collNormal = point2D.normal;

        if (Mathf.Abs(collNormal.y) > Mathf.Abs(collNormal.x))
        {
            if (collNormal.y > 0)
            {
                top = true;
            }
            else
            {
                bottom = true;
            }
        }

        if (top)
        {
            Vector3 size = _teleportedPlayer.GetSize();
            _teleportPosition = new Vector3(0, size.y / 2);
        }
        else if (bottom)
        {
            Vector3 size = _teleportedPlayer.GetSize();
            _teleportPosition = new Vector3(0, -size.y / 2);
        }

        _teleportPosition += transform.position;

        LimbRB.constraints = RigidbodyConstraints2D.FreezeAll;

        _teleportedPlayer.ZeroVelocity();
        _teleportedPlayer.transform.position = _teleportPosition;
        ServiceLocator.Get<LimbManager>().RemoveLimb(this);
        ServiceLocator.Get<CameraManager>().RemovePlayer(gameObject);
        Destroy(gameObject);
    }
}

