using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    private bool _fadeIn = false;
    private bool _fadeOut = false;

    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration;
    private MapManager _mapManager;
    private GameManager _gm;

    private float _currentAlpha = 0f;
    private float _elapsedTime = 0f;

    private void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _mapManager = ServiceLocator.Get<MapManager>();
        _gm = ServiceLocator.Get<GameManager>();

        Color startColor = _fadeImage.color;
        startColor.a = 0f;
        _fadeImage.color = startColor;
    }

    public void StartTransition()
    {
        _fadeOut = true;
    }

    private void Update()
    {
        if (_fadeOut)
        {
            _elapsedTime += Time.deltaTime;

            float lerpFactor = Mathf.Clamp01(_elapsedTime / _fadeDuration);

            _currentAlpha = Mathf.Lerp(0f, 1f, lerpFactor);

            Color newColor = _fadeImage.color;
            newColor.a = _currentAlpha;
            _fadeImage.color = newColor;

            if (lerpFactor >= 1f)
            {
                _fadeOut = false;
                _fadeIn = true;
                _elapsedTime = 0f;
                _currentAlpha = 1f;
            }
        }
        else if (_fadeIn)
        {
            _elapsedTime += Time.deltaTime;

            float lerpFactor = Mathf.Clamp01(_elapsedTime / _fadeDuration);

            _currentAlpha = Mathf.Lerp(1f, 0f, lerpFactor);

            Color newColor = _fadeImage.color;
            newColor.a = _currentAlpha;
            _fadeImage.color = newColor;

            if (lerpFactor >= 1f)
            {
                _fadeIn = false;
                _elapsedTime = 0f;
                _currentAlpha = 0f;
            }
        }
    }

    public float GetFadeDuration()
    {
        return _fadeDuration;
    }
}
