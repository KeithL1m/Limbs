using UnityEngine;

public class GasLimb : Limb
{
    private ParticleManager _particleManager;
    
    protected override void Initialize()
    {
        base.Initialize();
        _particleManager = ServiceLocator.Get<ParticleManager>();
        _specialLimbs = true;
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