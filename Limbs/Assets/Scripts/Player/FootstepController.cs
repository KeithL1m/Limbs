using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    private AudioManager _audioManager;
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] private float _minTimeBetweenFootsteps = 0.3f;
    [SerializeField] private float _maxTimeBetweenFootsteps = 0.6f; 

    private bool _isWalking = false; 
    private float _timeSinceLastFootstep;

    private void Awake()
    {
        GameLoader gl = ServiceLocator.Get<GameLoader>();
        gl.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    private void Update()
    {
        if (_isWalking)
        {
            if (Time.time - _timeSinceLastFootstep >= Random.Range(_minTimeBetweenFootsteps, _maxTimeBetweenFootsteps))
            {
                AudioClip footstepSound = _footstepSounds[Random.Range(0, _footstepSounds.Length)];
                _audioManager.PlaySound(footstepSound, transform.position, SoundType.SFX);

                _timeSinceLastFootstep = Time.time;
            }
        }
    }

    public void StartWalking()
    {
        _isWalking = true;
    }

    public void StopWalking()
    {
        _isWalking = false;
    }
}
