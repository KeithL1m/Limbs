using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum SoundType
{
    Music = 0,
    SFX = 1,
    None = 2
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _masterGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

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

        source.outputAudioMixerGroup = _masterGroup;

        source.transform.position = position;

        source.clip = audio;

        source.volume = Mathf.Log10(volume) * 20f;

        source.Play();
    }

    public void PlaySound(AudioClip audio, Vector3 position, SoundType type, float volume = 1f)
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

        switch(type)
        {
            case (SoundType.Music):
                {
                    source.outputAudioMixerGroup = _musicGroup;
                    break;
                }
            case (SoundType.SFX):
                {
                    source.outputAudioMixerGroup = _sfxGroup;
                    break;
                }
            default: break;
        }

        source.transform.position = position;

        source.clip = audio;

        source.volume = Mathf.Log10(volume) * 20f;

        source.Play();
    }

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20f);
    }
}
