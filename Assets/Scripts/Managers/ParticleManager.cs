using UnityEngine;

public class ParticleManager
{
    private const string GasPoolName = "GasBomb";
    private const string ExplosionName = "Explosion";
    private const string TeleportName = "Teleport";
    
    private ObjectPoolManager _objectPoolManager;
    
    public ParticleManager Initialize()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        return this;
    }

    public void PlayGas(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(GasPoolName);
        gas.transform.position.Set(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }
    
    public void PlayExplosionParticle(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(ExplosionName);
        gas.transform.position.Set(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }
    public void PlayTeleportParticle(Vector3 pos) 
    {
        var gas = _objectPoolManager.GetObjectFromPool(TeleportName);
        gas.transform.position.Set(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }
}
