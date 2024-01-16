using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLimb : Limb
{
    public GameObject gasCloudPrefab;

    void Start()
    {
        State = LimbState.PickUp;
        LimbRB = GetComponent<Rigidbody2D>();
        LimbRB.SetRotation(0);

        Trail.SetActive(false);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        Size = GetComponent<Collider2D>().bounds.size.y;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
        _rVMultiplier = _limbData._returnVelocityMultiplier;
        _specialLimb = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;
            
        GasExplode();
        
    }


    void GasExplode()
    {
        Debug.Log("FART");
        Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

