using UnityEngine;


[CreateAssetMenu(fileName = "Limb", menuName = "ScriptableObjects/Limb", order = 1)]
public class LimbData : ScriptableObject
{
    [Header("Stats")]
    public float _throwSpeed;
    public float _throwAngle;
    public float _knockback;
    public float _damage;
    public float _specialDamage;
    public float _returnVelocityMultiplier;
    public float _weight;
}
