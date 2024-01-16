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

        Vector3 offset = new();

        if (top)
        {
            Vector3 size = _attachedPlayer.GetSize();
            offset = new Vector3(0, size.y / 2);
        }
        else if (bottom)
        {
            Vector3 size = _attachedPlayer.GetSize();
            offset = new Vector3(0, -size.y / 2);
        }

        StartCoroutine(Wait());

        _attachedPlayer.ZeroVelocity();
        _attachedPlayer.transform.position = transform.position + offset;
        ServiceLocator.Get<LimbManager>().RemoveLimb(this);
        Destroy(gameObject);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
    }
}

