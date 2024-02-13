using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLimb : Limb
{
    [SerializeField] Collider2D _collider;
    float _ghostTimer = 0.7f;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _specialLimbs = false;
    }

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        _collider.enabled = false;
        //MAKING THE LIMB TRANSPARENT     
        MakeTransparent();
        yield return new WaitForSeconds(_ghostTimer);
        _collider.enabled = true;
        MakeNormal();
        yield break;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;
        if (string.CompareOrdinal(collision.gameObject.tag, "Player") == 0)
        {
            PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
            _healthPlayer.AddDamage(10);
        }
    }

    void MakeTransparent()
    {
        if (_sprite != null)
        {
              Color color = _sprite.color;
              // Set the transparency value
              color.a = 0.5f;
              // Assign the modified color back to the sprite renderer
              _sprite.color = color;
        }
    }

    void MakeNormal()
    {
        if (_sprite != null)
        {
            Color color = _sprite.color;
            // Set the transparency value
            color.a = 1.0f;
            // Assign the modified color back to the sprite renderer
            _sprite.color = color;
        }
    }
}
