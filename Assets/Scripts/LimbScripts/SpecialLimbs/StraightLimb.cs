using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLimb : Limb
{
    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        LimbRB.gravityScale = 0;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        LimbRB.gravityScale = 1;
    }
}
