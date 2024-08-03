using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Limb
{
    [SerializeField] private AudioClip _pickleSound;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (State != LimbState.Throwing)
        {
            return;
        }
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        _audioManager.PlaySound(_pickleSound, transform.position, SoundType.SFX, 0.5f);
        collision.rigidbody.AddForce(-_returnVelocity.normalized * _knockbackAmt);
    }
}
