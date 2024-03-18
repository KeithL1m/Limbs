using UnityEngine;

public class ParticleManager
{
    private const string GasPoolName = "GasBomb";
    private const string GasParticlePoolName = "GasParticle";
    private const string ExplosionName = "Explosion";
    private const string TeleportName = "Teleport";
    private const string ClashName = "Clash";
    private const string RespawnSmokeName = "RespawnSmoke";
    public const string ConfettiName = "Confetti";

    private ObjectPoolManager _objectPoolManager;
    
    public ParticleManager Initialize()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        return this;
    }

    public void PlayGas(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(GasPoolName);
        var gasParticle = _objectPoolManager.GetObjectFromPool(GasParticlePoolName);
        gas.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gasParticle.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gas.SetActive(true);
        gasParticle.SetActive(true);
    }
    
    public void PlayExplosionParticle(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(ExplosionName);
        gas.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }

    public void PlayRespawnParticle(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(RespawnSmokeName);
        gas.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }
    

    public void PlayTeleportParticle(Vector3 pos) 
    {
        var gas = _objectPoolManager.GetObjectFromPool(TeleportName);
        gas.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }

    public void PlaySwordClashParticle(Vector3 pos)
    {
        GameObject sword = _objectPoolManager.GetObjectFromPool(ClashName);
        sword.transform.position = new Vector3(pos.x, pos.y, pos.z);
        sword.SetActive(true);
    }

    public void PlayConfettiParticle(Vector3 pos) 
    {
        GameObject confetti = _objectPoolManager.GetObjectFromPool(ConfettiName);
        confetti.transform.position = new Vector3(pos.x, pos.y, pos.z);
        confetti.SetActive(true);
    }
}
