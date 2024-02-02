using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private ParticleSystem _meat;

    public void PlayDamageParticle()
    {
        _blood.Play();
        _meat.Play();
    }
}
