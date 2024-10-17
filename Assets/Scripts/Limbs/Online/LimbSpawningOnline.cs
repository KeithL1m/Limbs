using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class LimbSpawningOnline : NetworkBehaviour
{
    private GameLoader _loader;
    private GameManager _gm;

    private Transform _leftLimit;
    private Transform _rightLimit;

    private float _left;
    private float _right;

    private LimbManager _limbManager;

    [Header("Customizable")]
    [SerializeField] private List<GameObject> _limbOptions;

    private int _limbLimit;
    private int _startLimbCount;

    private double _minSpawnTimer;
    private double _maxSpawnTimer;
    private float _specialSpawnerMultipler = 5;

    private float _maxAngularVelocity;

    private int _currentLimbs;
    private float _limbTimer;

    private float _spawnPosX;
    private float _spawnPosY;

    private bool _specialSpawner;

    private static System.Random rnd = new System.Random();
    private bool _initialized = false;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _limbManager = ServiceLocator.Get<LimbManager>();

        SetLimbOptions();
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
            SpawnLimbRandomServerRpc();
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

        _currentLimbs = _limbManager.GetLimbAmount(_specialSpawner);

        if (_currentLimbs >= _limbLimit)
            return;

        _limbTimer -= Time.deltaTime;

        if (_limbTimer <= 0.0f && _limbOptions.Count > 0)
        {
            SpawnLimbRandomServerRpc();
            double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
            _limbTimer = (float)time;
        }
    }

    private void OnDisable()
    {
        foreach (var limb in _limbOptions)
        {
            NetworkManager.Singleton.RemoveNetworkPrefab(limb);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnLimbRandomServerRpc()
    {
        int index = rnd.Next(_limbOptions.Count);
        double val = rnd.NextDouble() * (_right - _left) + _left;
        double val2 = rnd.NextDouble() * _maxAngularVelocity;
        _spawnPosX = (float)val;

        GameObject gObj = Instantiate(_limbOptions[index], new Vector3(_spawnPosX, _spawnPosY, 0), Quaternion.identity);
        NetworkObject networkObject = gObj.GetComponent<NetworkObject>();
        Rigidbody2D limb = gObj.GetComponent<Rigidbody2D>();

        networkObject.Spawn();
        limb.angularVelocity = (float)val2;
    }

    private void ChangeLimbOptions()
    {
        _limbOptions = new List<GameObject>(_limbManager.GetLimbList());

        for (int i = _limbOptions.Count - 1; i >= 0; i--)
        {
            Limb limb = _limbOptions[i].GetComponent<Limb>();

            bool removeSpecial = limb.IsSpecial && !_specialSpawner;
            bool removeNormal = !limb.IsSpecial && _specialSpawner;

            if (removeNormal || removeSpecial)
            {
                _limbOptions.Remove(_limbOptions[i]);
                Debug.Log("removed item");
            }
        }
    }

    private void UpdateTimer()
    {
        _minSpawnTimer = _limbManager.GetMinSpawnTime();
        _maxSpawnTimer = _limbManager.GetMaxSpawnTime();

        if (_specialSpawner)
        {
            _minSpawnTimer *= _specialSpawnerMultipler;
            _maxSpawnTimer *= _specialSpawnerMultipler;
        }
    }

    private void UpdateLimit()
    {
        _limbLimit = _limbManager.GetLimbLimit();

        if (_specialSpawner)
        {
            _limbLimit /= 2;
        }
    }
    public void SetLimits(Transform leftLimit, Transform rightLimit)
    {
        _leftLimit = leftLimit;
        _rightLimit = rightLimit;
    }

    public void SetLimbOptions()
    {
        foreach (var limb in _limbOptions)
        {
            NetworkManager.Singleton.AddNetworkPrefab(limb);
        }
    }

    public void SetSpecs(int limbLimit, int startLimbCount, double minSpawnTimer, double maxSpawnTimer, float specialSpawnerMultipler, float maxAngularVelocity, bool specialSpawner)
    {
        _limbLimit = limbLimit;
        _startLimbCount = startLimbCount;
        _minSpawnTimer = minSpawnTimer;
        _maxSpawnTimer = maxSpawnTimer;
        _specialSpawnerMultipler = specialSpawnerMultipler;
        _maxAngularVelocity = maxAngularVelocity;
        _specialSpawner = specialSpawner;
    }
}