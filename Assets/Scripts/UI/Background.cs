using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameLoader _loader;
    private Transform _camera;
    private Vector3 _lastCamPos;
    private float _backgroundCount;

    [SerializeField] private List<Transform> _backgrounds;
    [SerializeField] private List<float> _parallaxMultipliers;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        _camera = ServiceLocator.Get<CameraManager>().GetCamera();
        _backgroundCount = _backgrounds.Count;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = _camera.position - _lastCamPos;

        for (int i = 0; i < _backgroundCount; i++)
        {
            _backgrounds[i].position += deltaMovement * _parallaxMultipliers[i];
        }

        _lastCamPos = _camera.position;
    }
}
