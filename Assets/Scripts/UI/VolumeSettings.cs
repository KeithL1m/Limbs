using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    public void SetMasterVolume(float volume)
    {
        _audioManager.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _audioManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        _audioManager.SetSFXVolume(volume);
    }
}
