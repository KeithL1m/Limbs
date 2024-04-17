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

    public void OnSelect(BaseEventData eventData)
    {
        _descriptionText.text = _descriptionToDisplay;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log(GetType() + "-" + name + "-OnDeselect();");
    }
}
