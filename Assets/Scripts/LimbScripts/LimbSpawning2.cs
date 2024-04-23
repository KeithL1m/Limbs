using System.Collections.Generic;
using UnityEngine;

public class LimbSpawning2 : MonoBehaviour
{
    private GameLoader _loader;
    private LimbManager _limbManager;

    [Header("Customizable")]
    [SerializeField] private List<GameObject> _limbOptions;
    [SerializeField]  private int _limbLimit;
    [SerializeField] private int _startLimbCount;

    [SerializeField]  private double _minSpawnTimer;
    [SerializeField] private double _maxSpawnTimer;

    [SerializeField] private float _maxAngularVelocity;

    private int _currentLimbs;
    private float _limbTimer;

    [SerializeField] private List<GameObject> _spawnPositions;
    [SerializeField] private bool _specialSpawner;

    private static System.Random rnd = new System.Random();

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _limbManager = ServiceLocator.Get<LimbManager>();

        _limbManager.Initialize();
        ChangeLimbOptions();
        UpdateTimer();
        UpdateLimit();
        _limbManager.ChangeChosenLimbs += ChangeLimbOptions;
        _limbManager.UpdateTime += UpdateTimer;

        for (int i = 0; i < _startLimbCount; i++)
        {
            SpawnLimbSpecific();
        }

        double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
        _limbTimer = (float)time;
    }

    private void Update()
    {
        _currentLimbs = _limbManager.GetLimbAmount();

        if (_currentLimbs >= _limbLimit)
            return;

        _limbTimer -= Time.deltaTime;

        if (_limbTimer <= 0.0f && _limbOptions.Count > 0)
        {
            SpawnLimbSpecific();
            double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
            _limbTimer = (float)time;
        }
    }

    private void SpawnLimbSpecific()
    {
        int index = rnd.Next(_limbOptions.Count);
        int spawnIndex = rnd.Next(_spawnPositions.Count);
        Vector3 position = _spawnPositions[spawnIndex].transform.position;
        double val2 = rnd.NextDouble() * _maxAngularVelocity;
        Rigidbody2D limb = Instantiate(_limbOptions[index], new Vector3(position.x, position.y, position.z), Quaternion.identity).GetComponent<Rigidbody2D>();
        limb.angularVelocity = (float)val2;
    }

    private void ChangeLimbOptions()
    {
        _limbOptions = _limbManager.GetLimbList();

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
            _minSpawnTimer *= 5f;
            _maxSpawnTimer *= 5f;
        }
    }

    private void UpdateLimit()
    {
        _limbLimit = _limbManager.GetLimbLimit();
    }
}
