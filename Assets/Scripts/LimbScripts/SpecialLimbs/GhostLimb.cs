using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLimb : Limb
{
    [SerializeField] Collider2D _collider;
    [SerializeField] private SpriteRenderer _ghostSprite;
    float _ghostTimer = 1.2f;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _ghostSprite = GetComponent<SpriteRenderer>();
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
        yield return new WaitForSeconds(_ghostTimer);

        //MAKING THE LIMB TRANSPARENT

        //if (_ghostSprite != null)
        //{
        //    if (!_collider.enabled)
        //    {
        //        Color color = _ghostSprite.color;
        //        // Set the transparency value
        //        color.a = 0.3f;
        //        // Assign the modified color back to the sprite renderer
        //        _ghostSprite.color = color;
        //    }
        //}


        _collider.enabled = true;
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
}
