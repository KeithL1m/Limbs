using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private TMP_Text _descriptionText;
    [SerializeField]
    private string _descriptionToDisplay;
    [SerializeField] 
    private AudioClip _audioClip;

    [SerializeField]
    private Animator _animator;

    public void OnSelect(BaseEventData eventData)
    {
        ServiceLocator.Get<AudioManager>().PlaySound(_audioClip, transform.position, SoundType.SFX);
        _descriptionText.text = _descriptionToDisplay;
        //_animator.SetTrigger("ButtonWobble");
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log(GetType() + "-" + name + "-OnDeselect();");
    }
}
