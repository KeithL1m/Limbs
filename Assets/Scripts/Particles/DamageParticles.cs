using UnityEngine;

public class DamageParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private ParticleSystem _meat;

    private void Awake()
    {
        var blood = _blood.main;
        blood.simulationSpace = ParticleSystemSimulationSpace.World;

        var meat = _meat.main;
        meat.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    public void PlayDamageParticle()
    {
        _blood.Play();
        _meat.Play();
    }
}
