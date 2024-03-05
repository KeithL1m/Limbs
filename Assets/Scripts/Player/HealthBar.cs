using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthFill;

    public void SetMaterial(Material material)
    {
        _portrait.material = material;
        _healthBar.material = material;
        _healthFill.material = material;
    }
}
