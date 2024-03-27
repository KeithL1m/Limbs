using System.Collections.Generic;
using UnityEngine;

public class LimbSpawning2 : MonoBehaviour
{
    /*
     * SPAWN LIMBS USING SPAWN POINTS 
     */
    GameLoader _loader;

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

    private int _currentLimbs;
    private float _limbTimer;

    [SerializeField]
    private List<GameObject> _spawnPositions;


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

        if (_limbTimer <= 0.0f)
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
        Limb limb = Instantiate(_limbOptions[index], new Vector3(position.x, position.y, position.z), Quaternion.identity).GetComponent<Limb>();
    }
}
