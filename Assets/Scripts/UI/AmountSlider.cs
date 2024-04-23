using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AmountSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private LimbManager _lm;

    [SerializeField] private float _amount;
    private float _amountScale;

    private void Start()
    {
        _slider.onValueChanged.AddListener(delegate { UpdateAmount(); });
        _amountScale = _amount / _slider.value;
        _lm = ServiceLocator.Get<LimbManager>();
    }

    private void UpdateAmount()
    {
        _amount = _slider.value * _amountScale;

        if (_slider.value < 0.1f)
        {
            _amount = 0.1f * _amountScale;
        }

        _lm.SetMaxAmount((int) _amount);
    }
}
