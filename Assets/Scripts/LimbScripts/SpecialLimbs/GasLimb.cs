using System;
using System.Collections;
using UnityEngine;

public class GasLimb : Limb
{
    private ParticleManager _particleManager;

    float _delayTimer = 0.0001f;
    [SerializeField] Collider2D _collider;

    protected override void Initialize()
    {
        base.Initialize();
        _particleManager = ServiceLocator.Get<ParticleManager>();
        _specialLimbs = true;
    }

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_delayTimer);
        _collider.enabled = true;
        yield break;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;

        GasExplode();
    }

    void GasExplode()
    {
        Debug.Log("FART");
        _particleManager.PlayGas(transform.position);
        ServiceLocator.Get<LimbManager>().RemoveLimb(this);
        Destroy(gameObject);
    }
}