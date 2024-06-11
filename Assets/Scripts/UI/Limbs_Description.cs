using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Limbs_Description : MonoBehaviour
{
    [SerializeField] private TMP_Text _limbNameText;
    [SerializeField] private TMP_Text _limbDescriptionText;
    [SerializeField] private Image _limbSprite;

    public void UpdateInfo(LimbUIInfo info)
    {
        _limbSprite.sprite = info.limbSprite;
        _limbNameText.text = info.limbName;
        _limbDescriptionText.text = info.limbDescription;
    }
}
