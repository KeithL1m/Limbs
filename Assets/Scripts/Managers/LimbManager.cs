using System;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : Manager
{
    [SerializeField] private List<GameObject> _limbOptions;
    [SerializeField] private double _maxTime;
    [SerializeField] private double _minTime;
    private double _currentTime;

    private List<Limb> _limbs;
    private bool _initialized = false;

    public Action ChangeChosenLimbs;

    public void SetLimbOptions(List<GameObject> limbs)
    {
        _limbOptions = limbs;
    }

    public void Initialize()
    {
        if (!_initialized)
        {
            _limbs = new List<Limb>();

            _initialized = true;
        }
    }

    void LateUpdate()
    {
        if (!_initialized)
        {
            return;
        }

        //make object pool for this
        for (int i = 0; i < _limbs.Count; i++)
        {
            _limbs[i].LimbUpdate();
        }
    }

    public void AddLimb(Limb limb)
    {
        _limbs.Add(limb);
    }

    public void RemoveLimb(Limb limb)
    {
        _limbs.Remove(limb);
    }

    public void ClearList()
    {
        _limbs.Clear();
    }

    public int GetLimbAmount()
    {
        return _limbs.Count;
    }

    public List<GameObject> GetLimbList()
    {
        return _limbOptions;
    }

    public void RemoveFromChosen(GameObject connectedLimb)
    {
        _limbOptions.Remove(connectedLimb);
        ChangeChosenLimbs?.Invoke();
    }

    public void AddToChosen(GameObject connectedLimb)
    {
        if (!_limbOptions.Contains(connectedLimb))
        {
            _limbOptions.Add(connectedLimb);
            ChangeChosenLimbs?.Invoke();
        }
    }
    public void SetMinSpawnTime(double time)
    {
        _minTime = time;
    }

    public void SetMaxSpawnTime(double time)
    {
        _maxTime = time;
    }

    public double GetMinSpawnTime()
    {
        return _currentTime - 1.5;
    }

    public double GetMaxSpawnTime()
    {
        return _currentTime + 1.5;
    }
}
