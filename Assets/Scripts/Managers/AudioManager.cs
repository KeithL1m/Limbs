using UnityEngine;
using System.Collections.Generic;

public enum SoundType
{
    Music = 0,
    SFX = 1
}

public class AudioManager : MonoBehaviour
{
    public float MusicVolume;
    public float SoundFXVolume;

    [SerializeField] private List<AudioSource> _sources = new();

    private void Awake()
    {
        GameLoader gm = ServiceLocator.Get<GameLoader>();
        gm.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        
    }

    //make function for random sound?

    public void PlaySound(AudioClip audio, Vector3 position, float volume = 1f)
    {
        //move source to position if not already playing sound
        AudioSource source = new();
        for (int i = 0; i < _sources.Count; i++)
        {
            if (!_sources[i].isPlaying)
            {
                source = _sources[i];
            }
            else if (i + 1 == _sources.Count)
            {
                AudioSource newSource = new AudioSource();
                _sources.Add(newSource);
                source = newSource;
                break;
            }
        }

        source.transform.position = position;

        source.clip = audio;

        source.volume = volume;

        source.Play();

        Destroy(source, 2);
    }

    public void PlaySound(AudioClip audio, Vector3 position, SoundType type, float volume = 1f)
    {
        float newVolume = 1;

        switch(type)
        {
            case SoundType.Music:
                {
                    newVolume = MusicVolume * volume;
                    break;
                }
            case SoundType.SFX:
                {
                    newVolume = SoundFXVolume * volume;
                    break;
                }
        }

        PlaySound(audio, position, newVolume);
    }
}
