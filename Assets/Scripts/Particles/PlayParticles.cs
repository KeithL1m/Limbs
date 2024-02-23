using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles1;
    [SerializeField] private ParticleSystem _particles2;
    [SerializeField] private ParticleSystem _particles3;
    [SerializeField] private ParticleSystem _particles4;

    [SerializeField] private float _extraTime;

    private bool _active;

    private float _currentTime;

    private void OnEnable()
    {
        _particles1.Play();
        if (_particles2 != null)
        {
            _particles2.Play();
        }
        if (_particles3 != null)
        {
            _particles3.Play();
        }
        if (_particles4 != null)
        {
            _particles4.Play();
        }

        _active = true;
        _currentTime = _particles1.main.duration + _extraTime;
    }

    private void OnDisable()
    {
        _particles1.Stop();
        if (_particles2 != null)
        {
            _particles2.Stop();
        }
        if (_particles3 != null)
        {
            _particles3.Stop();
        }
        if (_particles4 != null)
        {
            _particles4.Stop();
        }
    }

    private void Update()
    {
        if (_active)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0.0f)
            {
                _active = false;
                gameObject.SetActive(false);
            }
        }
    }
}
