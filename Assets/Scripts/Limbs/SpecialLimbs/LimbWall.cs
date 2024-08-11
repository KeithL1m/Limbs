using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class LimbWall : Limb
{
    [SerializeField] private float changeTime = 1f;
    [SerializeField] private GameObject wall;

    public override void ThrowLimb(int direction)
    {
        isThrow = true;
        PickupTimer = 0.3f;
        CanPickUp = false;
        _attachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
        transform.SetParent(ServiceLocator.Get<EmptyDestructibleObject>().transform);

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

        _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier * 0.6f, -LimbRB.velocity.y * _rVMultiplier * 0.6f, 0f);

        StartCoroutine(CreateWall());
    }

    public IEnumerator CreateWall()
    {
        yield return new WaitForSeconds(changeTime);
        GameObject wall = Instantiate(this.wall, transform.position, transform.rotation);
        wall.transform.eulerAngles = new Vector3(0, 0, 0);
        ServiceLocator.Get<LimbManager>().RemoveLimb(this);
        Destroy(gameObject);
    }
    bool isThrow;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isThrow)
            base.OnTriggerEnter2D(collision);
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!isThrow)
            base.OnTriggerStay2D(collision);
    }
}
