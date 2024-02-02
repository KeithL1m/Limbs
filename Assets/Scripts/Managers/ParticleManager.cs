using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _gas;
    [SerializeField] private ParticleSystem _explosion;

    public void PlayGas()
    {
        _gas.Play();
    }
    public void PlayExplosionParticle()
    {
        _explosion.Play();
    }
}
