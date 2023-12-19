using System.Collections.Generic;
using UnityEngine;

public class LimbManager : Manager
{
    private List<Limb> _limbs;
    private bool _initialized = false;

    public void Initialize()
    {
        if (!_initialized)
        {
            _limbs = new List<Limb>();

            _initialized = true;
        }
        else
        {
            _limbs.Clear();
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
            Limb limb = _limbs[i];

            if (limb.State == Limb.LimbState.Attached && limb.AnchorPoint != null)
            {
                limb.AttachedUpdate();
            }
            else if (limb.State == Limb.LimbState.Throwing || limb.State == Limb.LimbState.Returning)
            {
                
                if (limb.LimbRB.velocity.magnitude < 4.0f)
                {
                    limb.EnterPickupState();
                }
            }
        }
    }

    public void AddLimb(Limb limb)
    {
        _limbs.Add(limb);
    }

    public void ClearList()
    {
        _limbs.Clear();
    }
}
