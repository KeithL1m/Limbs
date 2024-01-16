using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLimb : Limb
{
    public GameObject gasCloudPrefab;

    protected override void Start()
    {
        base.Start();
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

