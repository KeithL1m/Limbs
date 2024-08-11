using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BasicTestMovement : NetworkBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _moveSpeed = 5.0f;

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector3 moveDir = Vector3.zero;

        if(Input.GetKey(KeyCode.A))
        {
            moveDir.x -= Time.deltaTime * _moveSpeed;
        }

        if(Input.GetKey(KeyCode.D))
        {
            moveDir.x += Time.deltaTime * _moveSpeed;
        }

        _transform.position += moveDir;
    }
}
