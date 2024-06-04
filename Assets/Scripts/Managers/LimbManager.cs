using System;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : Manager
{
    [SerializeField] private List<GameObject> _limbOptions;
    [SerializeField] private double _timeRange;
    [SerializeField] private int _limbLimit;
    [ShowOnly] public float _currentTime = 3;

    private List<Limb> _limbs;
    private bool _initialized = false;

    public Action ChangeChosenLimbs;
    public Action UpdateTime;
    public Action UpdateAmount;

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

    public int GetLimbAmount(bool special)
    {
        int limbs = 0;
        for (int i = 0; i < _limbs.Count; i++)
        {
            if (special)
            {
                limbs += _limbs[i].IsSpecial ? 1 : 0;
            }
            else
            {
                limbs += _limbs[i].IsSpecial ? 0 : 1;
            }
        }
        return limbs;
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

    public void SetMaxAmount(int amount)
    {
        Debug.Log("Limit was changed");
        _limbLimit = amount;
        UpdateAmount?.Invoke();
    }

    public int GetLimbLimit()
    {
        return _limbLimit;
    }

    public void SetSpawnTime(float time, double range)
    {
        _timeRange = range;
        _currentTime = time;
        UpdateTime?.Invoke();
    }

    public double GetMinSpawnTime()
    {
        return _currentTime - _timeRange;
    }

    public double GetMaxSpawnTime()
    {
        return _currentTime + _timeRange;
    }
}
