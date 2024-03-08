using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Limb
{
    [SerializeField] private float _knockbackAmt;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        collision.rigidbody.AddForce(-_returnVelocity.normalized * _knockbackAmt);
    }
}
