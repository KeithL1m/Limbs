using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimbInfo : MonoBehaviour
{
    public enum LimbType
    {
        NORMAL,
        SPECIAL,
        STATUS,
        BLUNT,
        EXPLOSION,
        FLOATY,
        PICKLE,
        SPEED,
        AGRESSIVE,
        DEFENSE
    }
    // Limb description
    [SerializeField] private string _limbName;

    [SerializeField] private string _limbType;

    [TextArea]
    [SerializeField] private string _limbDescription;

    [SerializeField] private Sprite _limbSprite;
    
}
