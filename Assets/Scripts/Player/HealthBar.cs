using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public void SetColorAlpha(float alpha)
    {
        _portrait.color = new Color(_portrait.color.r, _portrait.color.g, _portrait.color.b, alpha);
        _healthBar.color = new Color(_healthBar.color.r, _healthBar.color.g, _healthBar.color.b, alpha);
        _healthFill.color = new Color(_healthFill.color.r, _healthFill.color.g, _healthFill.color.b, alpha);
    }
}
