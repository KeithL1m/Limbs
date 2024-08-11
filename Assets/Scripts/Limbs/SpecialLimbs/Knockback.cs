using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Limb
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        collision.rigidbody.AddForce(-_returnVelocity.normalized * _knockbackAmt);
    }
}
