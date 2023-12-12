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

            if (limb.State == Limb.LimbState.PickUp)
            {
                if (limb.LimbTimer)
                {
                    limb.CanNotPickUp -= Time.deltaTime;
                }
                continue;
            }

            if (limb.State == Limb.LimbState.Attached && limb.AnchorPoint != null)
            {
                limb.transform.position = limb.AnchorPoint.position;
                limb.Trail.SetActive(false);
            }
            else if (limb.State == Limb.LimbState.Throwing || limb.State == Limb.LimbState.Returning)
            {
                if (limb.State == Limb.LimbState.Returning)
                {
                    if (limb.LimbTimer)
                    {
                        limb.CanNotPickUp -= Time.deltaTime;
                    }
                }
                if (limb.LimbRB.velocity.magnitude < 4.0f)
                {
                    limb.Flip(1);
                    Physics2D.IgnoreCollision(limb.AttachedPlayer.GetComponent<Collider2D>(), limb.GetComponent<Collider2D>(), false);
                    limb.Trail.SetActive(false);
                    limb.PickUpIndicator.SetActive(true);
                    limb.State = Limb.LimbState.PickUp;
                    limb.AttachedPlayer = null;
                    limb.AttachedPlayerLimbs = null;
                    limb.LimbTimer = true;
                }
            }
        }
    }

    public void AddLimb(Limb limb)
    {
        _limbs.Add(limb);
    }
}
