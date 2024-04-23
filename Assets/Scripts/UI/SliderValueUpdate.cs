using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueUpdate : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderText;

    void Awake()
    {
        _slider = GetComponentInParent<Slider>();
        _sliderText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        UpdateText(_slider.value);
        _slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float val)
    {
        _sliderText.text = _slider.value.ToString();
    }
}
