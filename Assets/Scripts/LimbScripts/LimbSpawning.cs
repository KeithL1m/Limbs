using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LimbSpawning : MonoBehaviour
{
    private GameLoader _loader;
    private GameManager _gm;

    public int playerCount;

    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;

    private float _left;
    private float _right;

    private LimbManager _limbManager;

    [Header("Customizable")]
    [SerializeField] private List<GameObject> _limbOptions;
    
    [SerializeField] private int _limbLimit;
    [SerializeField] private int _startLimbCount;

    [SerializeField] private double _minSpawnTimer;
    [SerializeField] private double _maxSpawnTimer;

    [SerializeField] private float _maxAngularVelocity;

    private int _currentLimbs;
    private float _limbTimer;

    private float _spawnPosX;
    private float _spawnPosY;

    [SerializeField] private bool _specialSpawner;

    private static System.Random rnd = new System.Random();
    private bool _initialized = false;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();
        _limbManager = ServiceLocator.Get<LimbManager>();

        _limbManager.Initialize();
        ChangeLimbOptions();
        UpdateTimer();
        UpdateLimit();
        _limbManager.ChangeChosenLimbs += ChangeLimbOptions;
        _limbManager.UpdateTime += UpdateTimer;
        _limbManager.UpdateAmount += UpdateLimit;

        _left = _leftLimit.position.x;
        _right = _rightLimit.position.x;

        _spawnPosY = transform.position.y;

        for (int i = 0; i < _startLimbCount; i++)
        {
            double val = rnd.NextDouble() * (_right - _left) + _left;
            _spawnPosX = (float)val;
            SpawnLimbRandom();
        }

        double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
        _limbTimer = (float)time;
        _initialized = true;
    }

    private void Update()
    {
		if (!_initialized)
        {
            return;
        }

        _currentLimbs = _limbManager.GetLimbAmount();

        if (_currentLimbs >= _limbLimit)
            return;
        
        _limbTimer -= Time.deltaTime;

        if (_limbTimer <= 0.0f && _limbOptions.Count > 0)
        {
            SpawnLimbRandom();
            double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
            _limbTimer = (float)time;
        }
    }

    private void SpawnLimbRandom()
    {
        if (playerCount <= 1)
        {
            playerCount = _gm.GetPlayerCount();
            return;
        }
        
        int index = rnd.Next(_limbOptions.Count);
        double val = rnd.NextDouble() * (_right - _left) + _left;
        double val2 = rnd.NextDouble() * _maxAngularVelocity;
        _spawnPosX = (float)val;
        Rigidbody2D limb = Instantiate(_limbOptions[index], new Vector3(_spawnPosX, _spawnPosY, 0), Quaternion.identity).GetComponent<Rigidbody2D>();
        limb.angularVelocity = (float)val2;
    }

    private void ChangeLimbOptions()
    {
        _limbOptions = _limbManager.GetLimbList();

        foreach( var item in _limbOptions )
        {
            Limb limb = item.GetComponent<Limb>();

            bool remove = limb.IsSpecial && !_specialSpawner || !limb.IsSpecial && _specialSpawner;

            if (remove)
            {
                _limbOptions.Remove(item);
            }
        }
    }

    private void UpdateTimer()
    {
        _minSpawnTimer = _limbManager.GetMinSpawnTime();
        _maxSpawnTimer = _limbManager.GetMaxSpawnTime();

        if (_specialSpawner)
        {
            _minSpawnTimer *= 5f;
            _maxSpawnTimer *= 5f;
        }
    }

    private void UpdateLimit()
    {
        _limbLimit = _limbManager.GetLimbLimit();
    }
}
