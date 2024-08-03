using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadoutButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioClip _browseSound;
    AudioManager _am;

    private void Awake()
    {
        GameLoader gl = ServiceLocator.Get<GameLoader>();
        gl.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _am = ServiceLocator.Get<AudioManager>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        _am.PlaySound(_browseSound, transform.position, SoundType.SFX);
    }
}
