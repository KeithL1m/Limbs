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

        if (PickupTimer <= 0.0f)
        {
            LimbRB.gravityScale = 1;
        }
    }

    protected override void EnterPickupState()
    {
        base.EnterPickupState();

        LimbRB.gravityScale = 1;
    }
}
