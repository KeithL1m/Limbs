using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform _target;
    //public Player _playerTarget;
    public float _hideDistance;

    void Update()
    {
        var dir = _target.transform.position - transform.position;
        if(dir.magnitude < _hideDistance)
        {
            SetChildActive(false);
        }
        else
        {
            SetChildActive(true);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetChildActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}
