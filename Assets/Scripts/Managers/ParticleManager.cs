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
    public const string RedExplosionName = "RedExplosion";
    public const string BreakableWallName = "BreakableWall";
    public const string DeathName = "Death";
    public const string DrunkLiquidBubbleBurstName = "DrunkLiquidBubbleBurst";
    public const string DrunkRisingBubbleName = "DrunkRisingBubble";
    public const string StickBombName = "StickBomb";


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


    public void PlayParticle(string name, Vector2 pos)
    {
        var particle = _objectPoolManager.GetObjectFromPool(name);
        particle.transform.position = new Vector3(pos.x, pos.y);
        particle.transform.position = new Vector3(pos.x, pos.y);
        particle.SetActive(true);
    }


    public void PlayTeleportParticle(Vector3 pos)
    {
        var gas = _objectPoolManager.GetObjectFromPool(TeleportName);
        gas.transform.position = new Vector3(pos.x, pos.y, pos.z);
        gas.SetActive(true);
    }

    public void PlayStickBombParticle(Vector3 pos)
    {
        var stickBomb= _objectPoolManager.GetObjectFromPool(StickBombName);
        stickBomb.transform.position = new Vector3(pos.x, pos.y, pos.z);
        stickBomb.SetActive(true);
    }

    public void PlaySwordClashParticle(Vector3 pos)
    {
        GameObject sword = _objectPoolManager.GetObjectFromPool(ClashName);
        sword.transform.position = new Vector3(pos.x, pos.y, pos.z);
        sword.SetActive(true);
    }
    public void PlayRedExplosionParticle(Vector3 pos)
    {
        GameObject redExplosion = _objectPoolManager.GetObjectFromPool(RedExplosionName);
        redExplosion.transform.position = new Vector3(pos.x, pos.y, pos.z);
        redExplosion.SetActive(true);
    }
    public void PlayConfettiParticle(Vector3 pos)
    {
        GameObject confetti = _objectPoolManager.GetObjectFromPool(ConfettiName);
        confetti.transform.position = new Vector3(pos.x, pos.y, pos.z);
        confetti.SetActive(true);
    }
    public void PlayBreakableWallParticle(Vector3 pos)
    {
        GameObject breakableWall = _objectPoolManager.GetObjectFromPool(BreakableWallName);
        breakableWall.transform.position = new Vector3(pos.x, pos.y, pos.z);
        breakableWall.SetActive(true);
    }
    public void PlayDrunkLiquidBubbleBurstParticle(Vector3 pos)
    {
        GameObject drunkLiquidBubbleBurst = _objectPoolManager.GetObjectFromPool(DrunkLiquidBubbleBurstName);
        drunkLiquidBubbleBurst.transform.position = new Vector3(pos.x, pos.y, pos.z);
        drunkLiquidBubbleBurst.SetActive(true);
    }
    public void PlayDeathParticle(Vector3 pos)
    {
        GameObject death = _objectPoolManager.GetObjectFromPool(DeathName);
        death.transform.position = new Vector3(pos.x, pos.y, pos.z);
        death.SetActive(true);
    }
    public void PlayDrunkRisingBubbleParticle(Vector3 pos)
    {
        GameObject drunkRisingBubble = _objectPoolManager.GetObjectFromPool(DrunkRisingBubbleName);
        drunkRisingBubble.transform.position = new Vector3(pos.x, pos.y, pos.z);
        drunkRisingBubble.SetActive(true);
    }
}
