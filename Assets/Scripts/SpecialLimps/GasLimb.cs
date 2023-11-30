using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasLimb : MonoBehaviour
{
    public GameObject gasCloudPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            GasExplode();
        }
    }


    void GasExplode()
    {
        Debug.Log("FART");
        Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

