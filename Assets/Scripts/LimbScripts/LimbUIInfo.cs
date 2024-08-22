using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LimbUIData", menuName = "ScriptableObjects/LimbUIData", order = 1)]
public class LimbUIInfo : ScriptableObject
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
        DEFENSE,
        NONE
    }

    public string limbName;

    [TextArea]
    public string limbDescription;

    public Sprite limbSprite;
    public enum limbType1 { };
    public enum limbType2 { };
}