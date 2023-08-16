using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Limb", menuName = "ScriptableObjects/Limb", order = 1)]
public class LimbData : ScriptableObject
{
    [Header("Stats")]
    public float _throwSpeed;
    public float _angularVelocity;
    public float _throwAngle;
    public float _knockback;
    public float _knockbackCounter;
    public float _knockbackLength;
    public float _damage;
    public float _specialDamage;
}
