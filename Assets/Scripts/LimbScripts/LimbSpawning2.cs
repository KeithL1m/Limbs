using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbSpawning2 : MonoBehaviour
{
    /*
     * SPAWN LIMBS USING SPAWN POINTS 
     */

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

    private void Start()
    {
        _limbManager= GetComponent<LimbManager>();

        for (int i = 0; i < _startLimbCount; i++)
        {
            SpawnLimb();
        }

        double time = rnd.NextDouble() * (_maxSpawnTimer - _minSpawnTimer) + _minSpawnTimer;
        _limbTimer = (float)time;
    }

    private void Update()
    {
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
        int index = rnd.Next(_limbOptions.Count);
        Vector3 position = _spawnPositions[index].transform.position;
        Limb limb = Instantiate(_limbOptions[index], new Vector3(position.x, position.y, position.z), Quaternion.identity).GetComponent<Limb>();
        _limbManager.AddLimb(limb);
        _currentLimbs++;
    }
}
