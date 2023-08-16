using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowIndicator : MonoBehaviour
{

    public List<PlayerInput> _players = new List<PlayerInput>();
    public Transform _target;

    public float _hideDistance;
    void Start()
    {

    }
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


