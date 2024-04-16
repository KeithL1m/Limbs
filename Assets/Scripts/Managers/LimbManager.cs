using System;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : Manager
{
    [SerializeField] private List<Limb> _limbOptions;

    private List<Limb> _limbs;
    private bool _initialized = false;

    public Action ChangeChosenLimbs;

    public void SetLimbOptions(List<Limb> limbs)
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

    public List<Limb> GetLimbList()
    {
        return _limbOptions;
    }

    public void RemoveFromChosen(Limb connectedLimb)
    {
        _limbOptions.Remove(connectedLimb);
    }

    public void AddToChosen(Limb connectedLimb)
    {
        _limbOptions.Add(connectedLimb);
    }
}
