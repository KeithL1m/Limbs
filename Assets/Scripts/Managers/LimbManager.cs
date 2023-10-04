using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : MonoBehaviour
{
    public List<Limb> _limbs;

    void Start()
    {
        _limbs = new List<Limb>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Limb");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            _limbs.Add(gameObjects[i].GetComponent<Limb>());
        }
    }

    void Update()
    {
        for (int i = 0; i < _limbs.Count; i++)
        {
            if (_limbs[i]._limbState == Limb.LimbState.Attached && _limbs[i]._anchorPoint != null)
            {
                _limbs[i].transform.position = _limbs[i]._anchorPoint.position;
                _limbs[i]._trail.SetActive(false);
            }
            else if (_limbs[i]._limbState == Limb.LimbState.Throwing || _limbs[i]._limbState == Limb.LimbState.Returning)
            {
                _limbs[i]._attachedPlayerLimbs.RemoveLimb(_limbs[i]);
                if (_limbs[i].GetComponent<Rigidbody2D>().velocity.magnitude < 4.0f)
                {
                    Physics2D.IgnoreCollision(_limbs[i]._attachedPlayer.GetComponent<Collider2D>(), _limbs[i].GetComponent<Collider2D>(), false);
                    _limbs[i]._trail.SetActive(false);
                    _limbs[i]._pickUpIndicator.SetActive(true);
                    _limbs[i]._limbState = Limb.LimbState.PickUp;
                    _limbs[i]._attachedPlayer = null;
                    _limbs[i]._attachedPlayerLimbs = null;
                }
            }
        }
    }

    public void AddLimb(Limb limb)
    {
        _limbs.Add(limb);
    }
}
