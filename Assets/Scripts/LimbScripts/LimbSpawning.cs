using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimbSpawning : MonoBehaviour
{
    /*
     * SPAWN LIMBS IN A RANGE
     */
    GameLoader _loader;
    GameManager _gm;

    public List<PlayerInput> playerList = new List<PlayerInput>();

    public int playerCount;

    [SerializeField]
    private Transform _leftLimit;
    private float _left;
    [SerializeField]
    private Transform _rightLimit;
    private float _right;

    [SerializeField]
    private LimbManager _limbManager;

    [Header("Customizable")]
    [SerializeField]
    private List<GameObject> _limbOptions;
    [SerializeField]
    private int _limbLimit;
    [SerializeField]
    private int _startLimbCount;

    [SerializeField]
    private double _minSpawnTimer;
    [SerializeField]
    private double _maxSpawnTimer;

    [SerializeField]
    private float _maxAngularVelocity;

    private int _currentLimbs;
    private float _limbTimer;

    private float _spawnPosX;
    private float _spawnPosY;

    private static System.Random rnd = new System.Random();

    private bool _initialized = false;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();

        _limbManager.Initialize();

        _left = _leftLimit.position.x;
        _right = _rightLimit.position.x;

        _spawnPosY = transform.position.y;

        for (int i = 0; i < _startLimbCount; i++)
        {
            double val = rnd.NextDouble() * (_right - _left) + _left;
            _spawnPosX = (float)val;
            SpawnLimb();
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

        if (_currentLimbs < _limbLimit)
        {
            _limbTimer -= Time.deltaTime;
        }

        if (_limbTimer <= 0.0f)
        {
            SpawnLimb();
            double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
            _limbTimer = (float)time;
        }
    }

    private void SpawnLimb()
    {
        playerCount = _gm.GetPlayerCount();

        if(playerCount > 1)
        {
            int index = rnd.Next(_limbOptions.Count);
            double val = rnd.NextDouble() * (_right - _left) + _left;
            double val2 = rnd.NextDouble() * _maxAngularVelocity;
            _spawnPosX = (float)val;
            Limb limb = Instantiate(_limbOptions[index], new Vector3(_spawnPosX, _spawnPosY, 0), Quaternion.identity).GetComponent<Limb>();
            limb.GetComponent<Rigidbody2D>().angularVelocity = (float)val2;
            _limbManager.AddLimb(limb);
            _currentLimbs++;
        }
    }
}
