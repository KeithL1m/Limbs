using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : Manager
{
    private List<Limb> _limbs;
    private bool _initialized = false;

    public void Initialize()
    {
        _limbs = new List<Limb>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Limb");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Limb limb = gameObjects[i].GetComponent<Limb>();
            if(limb != null)
            {
                _limbs.Add(limb);
            }
        }
		
        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
        {
            return;
        }

        //make object pool for this
        for (int i = 0; i < _limbs.Count; i++)
        {
            if (_limbs[i].State == Limb.LimbState.PickUp)
                continue;
            if (_limbs[i].State == Limb.LimbState.Attached && _limbs[i].AnchorPoint != null)
            {
                _limbs[i].transform.position = _limbs[i].AnchorPoint.position;
                _limbs[i].Trail.SetActive(false);
            }
            else if (_limbs[i].State == Limb.LimbState.Throwing || _limbs[i].State == Limb.LimbState.Returning)
            {
                if (_limbs[i].LimbRB.velocity.magnitude < 4.0f)
                {
                    Physics2D.IgnoreCollision(_limbs[i].AttachedPlayer.GetComponent<Collider2D>(), _limbs[i].GetComponent<Collider2D>(), false);
                    _limbs[i].Trail.SetActive(false);
                    _limbs[i].PickUpIndicator.SetActive(true);
                    _limbs[i].State = Limb.LimbState.PickUp;
                    _limbs[i].AttachedPlayer = null;
                    _limbs[i].AttachedPlayerLimbs = null;
                }
            }
        }
    }

    public void AddLimb(Limb limb)
    {
        _limbs.Add(limb);
    }
}
