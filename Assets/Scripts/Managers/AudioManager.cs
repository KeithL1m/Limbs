using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.Timeline;
using System.Threading.Tasks;
using Microsoft.Cci;

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

    [SerializeField] private GameObject _basicAudioSource;

    public float MusicVolume;
    public float SoundFXVolume;

    [SerializeField] private List<AudioSource> _sources = new();
    private AudioSource _currentMusic;
    private AudioClip[] _gameMusic;
    [SerializeField] private int _numSources;

    private GameManager _gm;

    private bool _inTitle = false;
    private bool _inMeatcase = false;
    private bool _inGame = false;

    private float _songTime;

    private void Awake()
    {
        GameLoader gm = ServiceLocator.Get<GameLoader>();
        gm.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _audioMixer.GetFloat("Music Volume", out MusicVolume);
        _audioMixer.GetFloat("SFX Volume", out SoundFXVolume);
        _gm = ServiceLocator.Get<GameManager>();

        for (int i = 0; i < _numSources; i++)
        {
            AudioSource newSource = Instantiate(_basicAudioSource, _gm.transform).GetComponent<AudioSource>();
            _sources.Add(newSource);
        }
    }

    private void Update()
    {
        if (_inTitle)
        {
            _songTime -= Time.deltaTime;
            if (_songTime <= 0.0f)
            {
                StartTitleMusic(_currentMusic.clip);
            }
            
        }
        else if (_inMeatcase)
        {
            _songTime -= Time.deltaTime;

            if (_songTime <= 0.0f)
            {
                MeatcaseMusic(_currentMusic.clip);
            }
        }
        else if (_inGame)
        {
            _songTime -= Time.deltaTime;

            if (_songTime <= 0.0f)
            {
                GameMusic(_gameMusic);
            }
        }
    }

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
                AudioSource newSource = Instantiate(_basicAudioSource, _gm.transform).GetComponent<AudioSource>();
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

    //Volume must be in between 0 and 1
    public void PlaySound(AudioClip audio, Vector3 position, SoundType type, float volume = 1f)
    {
        //move source to position if not already playing sound
        AudioSource source = new();
        for (int i = 0; i < _sources.Count; i++)
        {
            if (!_sources[i].isPlaying)
            {
                source = _sources[i];
                break;
            }
            else if (i + 1 == _sources.Count)
            {
                AudioSource newSource = Instantiate(_basicAudioSource, _gm.transform).GetComponent<AudioSource>();
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
                    _currentMusic = source;
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

        source.volume = volume;

        source.Play();
    }

    public void PlayRandomSound(AudioClip[] audio, Vector3 position, SoundType type, float volume = 1f)
    {
        int clip = Random.Range(0, audio.Length);
        PlaySound(audio[clip], position, type, volume);
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

    public void StartTitleMusic(AudioClip audio)
    {
        _currentMusic?.Stop();
        _inTitle = true;

        PlaySound(audio, Vector3.zero, SoundType.Music, 0.3f);
        float duration = audio.length;
        _songTime = duration ;
    }

    public void MeatcaseMusic(AudioClip audio)
    {
        _currentMusic?.Stop();

        _inTitle = false;
        _inMeatcase = true;
        PlaySound(audio, Vector3.zero, SoundType.Music, 0.3f);
        float duration = audio.length;
        _songTime = duration;
    }

    public void GameMusic(AudioClip[] clips)
    {
        _currentMusic?.Stop();

        _inMeatcase = false;
        _inGame = true;
        if (_gameMusic == null)
        {
            _gameMusic = clips;
        }
        int num = Random.Range(0, _gameMusic.Length);
        PlaySound(clips[num], Vector3.zero, SoundType.Music, 0.3f);
        float duration = clips[num].length;
        _songTime = duration;
    }

    public void StopMusic()
    {
        _currentMusic?.Stop();
        _inGame = false;
    }
}
