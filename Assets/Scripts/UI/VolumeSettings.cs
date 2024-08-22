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
        if (volume == 0.0f)
        {
            volume = 1.0f;
        }
        float vol = volume * 0.01f;
        _audioManager.SetMasterVolume(vol);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume == 0.0f)
        {
            volume = 1.0f;
        }
        float vol = volume * 0.01f;
        _audioManager.SetMusicVolume(vol);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0.0f)
        {
            volume= 1.0f;
        }
        float vol = volume * 0.01f;
        _audioManager.SetSFXVolume(vol);
    }
}
