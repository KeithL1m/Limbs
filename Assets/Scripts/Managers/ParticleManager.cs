using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Manager
{
    //Gas particle
    [SerializeField] private ParticleSystem _gas;

    //Explosion particle
    [SerializeField] private ParticleSystem _explosion;

    //Teleport particle
    [SerializeField] private ParticleSystem _teleport;

    public ParticleManager Initialize()
    {
        return this;
    }

    public void PlayGas()
    {
        _gas.Play();
    }
    public void PlayExplosionParticle()
    {
        _explosion.Play();
    }
    public void PlayTeleportParticle() 
    {
        _teleport.Play();
    }
}
