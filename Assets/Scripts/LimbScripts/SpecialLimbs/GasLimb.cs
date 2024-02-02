using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLimb : Limb
{
    public GameObject gasCloudPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _specialLimbs = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

