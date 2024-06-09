using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    [SerializeField] private List<GameObject> _limbList;
    [SerializeField] private int _maxAmount;
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private float _spawnTimeRange = 1f;

    public void Setup()
    {
        var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.SetLimbOptions(_limbList);
        limbManager.SetMaxAmount(_maxAmount);
        limbManager.SetSpawnTime(_spawnTime, _spawnTimeRange);
    }
}
