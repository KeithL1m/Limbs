using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   // public CameraFocus _focusLevel;

    public List<GameObject> _players;

    public Bounds _maxBounds;
    private Bounds _playerBounds;
    private Vector3 _velocity;

    private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] public float _smoothZoomTime = 0.5f;
    public float _minHeight = 12.5f;
    public float _maxHeight;

    public float _heightMultiplier = 1.33f;
    public float _zoomLimiter;
    private float _zoomVelocity;

    private GameLoader _gameLoader = null;
    private PlayerManager _playerManager = null;
    private Camera _camera;
    private bool _initialized = false;

    void Start()
    {
        _gameLoader = ServiceLocator.Get<GameLoader>();
        _gameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("Camera Manager Initializing");

        //get the max extents of the camera
        _camera = GetComponent<Camera>();

        Vector3 cameraPosition = transform.position;

        float cameraHalfWidth = _camera.orthographicSize * _camera.aspect;
        float cameraHalfHeight = _camera.orthographicSize;

        Vector3 boundsMin = cameraPosition - new Vector3(cameraHalfWidth, cameraHalfHeight, 0f);
        Vector3 boundsMax = cameraPosition + new Vector3(cameraHalfWidth, cameraHalfHeight, 0f);

        _maxBounds = new Bounds(cameraPosition, Vector3.zero);
        _maxBounds.min = boundsMin;
        _maxBounds.max = boundsMax;

        _maxHeight = cameraHalfWidth * 2f;

        _offset = new Vector3(0, 0, -10);

        // Add players
        _playerManager = ServiceLocator.Get<PlayerManager>();
        var playerObjects = _playerManager.GetPlayerObjects();
        Debug.Log($"Camera is tracking {playerObjects.Count} players");
        _players.AddRange(playerObjects);
        _initialized = true;
    }

    private void LateUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        MoveCamera();
        AdjustCameraSize();
    }

    private void MoveCamera()
    {
        Vector3 centrePoint = CalculateCentrePoint();

        Vector3 newPos = centrePoint + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, _smoothTime);
    }

    private void AdjustCameraSize()
    {
        float distanceX = _playerBounds.size.x * 1.25f;
        float distanceY = _playerBounds.size.y * _heightMultiplier;

        distanceX = distanceX / _camera.aspect;

        float selectedDistance = distanceX;

        if (distanceY > selectedDistance)
        {
            selectedDistance = distanceY;
        }

        if (selectedDistance > _maxHeight)
        {
            selectedDistance = _maxHeight;
        }
        else if (selectedDistance < _minHeight)
        {
            selectedDistance = _minHeight;
        }

        selectedDistance *= 0.5f;

        //float newDistance = Mathf.Lerp(_minHeight, _maxHeight, selectedDistance / _maxHeight);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, selectedDistance, ref _zoomVelocity, _smoothZoomTime);
    }

    private Vector3 CalculateCentrePoint()
    {
        _playerBounds = new Bounds();
        for (int i = 0; i < _players.Count; i++)
        {
            _playerBounds.Encapsulate(_players[i].transform.position);
        }

        if (_playerBounds.min.x < _maxBounds.min.x)
        {
            _playerBounds.min = new Vector3(_maxBounds.min.x, _playerBounds.min.y, _playerBounds.min.z);
        }
        if (_playerBounds.min.y < _maxBounds.min.y)
        {
            _playerBounds.min = new Vector3(_playerBounds.min.x, _maxBounds.min.y, _playerBounds.min.z);
        }
        if (_playerBounds.max.x > _maxBounds.max.x)
        {
            _playerBounds.max = new Vector3(_maxBounds.max.x, _playerBounds.max.y, _playerBounds.max.z);
        }
        if (_playerBounds.max.y > _maxBounds.max.y)
        {
            _playerBounds.max = new Vector3(_playerBounds.max.x, _maxBounds.max.y, _playerBounds.max.z);
        }

        return _playerBounds.center;
    }
}