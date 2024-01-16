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
        Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

public class GasCloud : MonoBehaviour
{
    private float _gasRadiusRadius = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // Apply gas effect (reduce player health)
        }
    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _gasRadiusRadius);
    }
}
