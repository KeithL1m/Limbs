using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLimb : Limb
{
    [SerializeField] Collider2D _collider;
    float _ghostTimer = 1.2f;

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
        yield return new WaitForSeconds(_ghostTimer);
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
        PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
        _healthPlayer.AddDamage(10);
    }
}
