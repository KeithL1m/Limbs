using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   // public CameraFocus _focusLevel;

    public List<GameObject> _players;

    private Bounds _maxBounds;
    private Bounds _playerBounds;
    private Vector3 _velocity;

    private float _lastDistance = 0;
    private float _currentDistance = 0;

    [SerializeField] private Vector3 _offset = new Vector3(0, -1, -10);
    [SerializeField] private Vector3 _endOffset = new Vector3(0, 1.0f, -10);
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private float _smoothZoomInTime = 2f;
    [SerializeField] private float _smoothZoomOutTime = 0.7f;
    [SerializeField] private float _minHeight = 12.5f;
    [SerializeField] private float _endZoomDistance = 3.6f;
    [SerializeField] private float _endZoomTime = 0.7f;
    private float _maxHeight;
    private float _shakeIntensity;
    private float _shakeDuration;

    public float _heightMultiplier = 1.4f;
    public float _widthMultiplier = 1.4f;
    public float _zoomLimiter;
    private float _zoomVelocity;

    private GameLoader _gameLoader = null;
    private PlayerManager _playerManager = null;
    private Camera _camera;
    private GameManager _gameManager;
    private OptionsScreen _optionsScreen;
    private bool _initialized = false;
    private bool _teleportThrown = false;
    private bool _shaking = false;

    private Vector3 centrePoint = new Vector3();

    void Start()
    {
        _gameLoader = ServiceLocator.Get<GameLoader>();
        _gameLoader.CallOnComplete(Initialize);
        _optionsScreen = ServiceLocator.Get<OptionsScreen>();
    }

    private void Initialize()
    {
        Debug.Log("Camera Manager Initializing");

        ServiceLocator.Register<CameraManager>(this);
        _gameManager = ServiceLocator.Get<GameManager>();

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

        _maxHeight = cameraHalfHeight * 2f;

        //_offset = new Vector3(0, -1, -10);

        // Add players
        _playerManager = ServiceLocator.Get<PlayerManager>();
        var playerObjects = _playerManager.GetPlayerObjects();
        Debug.Log($"Camera is tracking {playerObjects.Count} players");
        _players.AddRange(playerObjects);

        _playerBounds = new Bounds();
        for (int i = 0; i < _players.Count; i++)
        {
            _playerBounds.Encapsulate(_players[i].transform.position);
        }

        _initialized = true;
    }

    private void LateUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        MoveCamera();

        if (_gameManager.IsGameOver)
        {
            EndZoom();
        }
        else
        {
            AdjustCameraSize();
        }

        if (_shaking)
        {
            Vector3 randomShake = Random.insideUnitSphere * _shakeIntensity;

            transform.position = transform.position + randomShake;

            _shakeDuration -= Time.deltaTime;

            if (_shakeDuration <= 0.0f)
            {
                _shaking = false;
            }
        }
    }

    public void StartScreenShake(float intensity, float duration)
    {
        _shakeIntensity = intensity;
        _shakeDuration = duration;
        _shaking = true;
        //if (_optionsScreen.screenShakeOptions == false)
        //{
        //    _shaking = false;
        //}
    }

    private void MoveCamera()
    {
        centrePoint = CalculateCentrePoint();

        Vector3 newPos = new Vector3();

        if (_gameManager.IsGameOver)
        {
            newPos = centrePoint + _endOffset;
        }
        else
        {
            newPos = centrePoint + _offset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, _smoothTime);
    }

    private void AdjustCameraSize()
    {
        float distanceX = _playerBounds.size.x * _widthMultiplier;
        float distanceY = _playerBounds.size.y * _heightMultiplier;

        distanceX = distanceX / _camera.aspect;

        _currentDistance = distanceX;

        if (distanceY > _currentDistance)
        {
            _currentDistance = distanceY;
        }

        if (_currentDistance > _maxHeight)
        {
            _currentDistance = _maxHeight;
        }
        else if (_currentDistance < _minHeight)
        {
          _currentDistance = _minHeight;
        }

        _currentDistance *= 0.5f;

        if (_teleportThrown)
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentDistance, ref _zoomVelocity, 0.1f);
        }
        else if (_currentDistance <= _lastDistance)
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentDistance, ref _zoomVelocity, _smoothZoomInTime);
           // Debug.Log("zoom in");
        }
        else
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentDistance, ref _zoomVelocity, _smoothZoomOutTime);
           // Debug.Log("zoom out");
        }

        _lastDistance = _camera.orthographicSize;
    }

    private Vector3 CalculateCentrePoint()
    {
        if (_gameManager.IsGameOver)
        {
            _playerBounds = new Bounds(_gameManager.GetWinningPlayer().transform.position, Vector3.zero);

        }
        else
        {
            _playerBounds = new Bounds(_players[0].transform.position, Vector3.zero);
            for (int i = 1; i < _players.Count; i++)
            {
                if (_players[i] != null)
                {
                    _playerBounds.Encapsulate(_players[i].transform.position);
                }
            }
        }

        //make sure camera doesn't move too far 

        return _playerBounds.center;
    }

    // Zoom to winning player
    private void EndZoom()
    { 
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _endZoomDistance, ref _zoomVelocity, _endZoomTime);
    }

    public void AddTeleport(GameObject player)
    {
        _players.Add(player);
        _teleportThrown = true;
    }

    public void RemoveTeleport(GameObject player)
    {
        _players.Remove(player);
        _teleportThrown = false;
    }

    public void Unregister()
    {
        ServiceLocator.Unregister<CameraManager>();
    }
}