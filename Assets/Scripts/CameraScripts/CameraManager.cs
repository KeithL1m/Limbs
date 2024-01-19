using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
   // public CameraFocus _focusLevel;

    public List<GameObject> _players;

    public Bounds _maxBounds;
    private Bounds _playerBounds;
    private Vector3 _velocity;

    public float _lastDistance = 0;
    public float _currentDistance = 0;

    private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] public float _smoothZoomInTime = 1f;
    [SerializeField] public float _smoothZoomOutTime = 0.2f;
    public float _minHeight = 12.5f;
    public float _maxHeight;

    public float _heightMultiplier = 1.7f;
    public float _widthMultiplier = 1.3f;
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

        ServiceLocator.Register<CameraManager>(this);

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

        _offset = new Vector3(0, -1, -10);

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

        if (_currentDistance <= _lastDistance)
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentDistance, ref _zoomVelocity, _smoothZoomInTime);
           // Debug.Log("zoom in");
        }
        else
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentDistance, ref _zoomVelocity, _smoothZoomOutTime);
           // Debug.Log("zoom out");
        }

        _lastDistance = _currentDistance;
    }

    private Vector3 CalculateCentrePoint()
    {
        _playerBounds = new Bounds();
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i] != null)
            {
                _playerBounds.Encapsulate(_players[i].transform.position);
            }
        }

        //make sure camera doesn't move too far 

        return _playerBounds.center;
    }

    public void AddPlayer(GameObject player)
    {
        _players.Add(player);
    }

    public void RemovePlayer(GameObject player)
    {
        _players.Remove(player);
    }

    public void Unregister()
    {
        ServiceLocator.Unegister<CameraManager>();
    }
}