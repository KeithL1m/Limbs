using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustedGravity : Limb
{
    [SerializeField] private float _gravityScale;
    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        LimbRB.gravityScale = _gravityScale;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (PickupTimer <= 0.0f)
        {
            LimbRB.gravityScale = 1;
        }
    }

    protected override void EnterPickupState()
    {
        base.EnterPickupState();

        LimbRB.gravityScale = _gravityScale;
    }
}
