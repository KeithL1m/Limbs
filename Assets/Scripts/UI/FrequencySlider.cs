using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private LimbManager _lm;

    [SerializeField] private float _defaultTime = 3f;
    [SerializeField] private float _range = 1f;
    private float timescale;
    private float rangeScale;

    private void Start()
    {
        _slider.onValueChanged.AddListener(delegate { UpdateTimer(); });
        timescale = _defaultTime / _slider.value;
        rangeScale = _range / _slider.value;
        _lm = ServiceLocator.Get<LimbManager>();
    }

    private void UpdateTimer()
    {
        _defaultTime = _slider.value * timescale;
        _range = _slider.value * rangeScale;

        if (_defaultTime < 0.5f)
        {
            _defaultTime = 0.5f;
            _range = 0.1f;
        }

        _lm.SetSpawnTime(_defaultTime, (double)_range);
    }
}
