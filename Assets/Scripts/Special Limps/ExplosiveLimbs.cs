using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLimbs : MonoBehaviour
{
    // Explosive informations
    Collider2D[] objectsInExplosion;
    [SerializeField] private float _explosionForce = 5.0f;
    [SerializeField] private float _delay = 3.0f;
    [SerializeField] private float _radius = 5.0f;

    float countdown;
    bool hasExploded = false;

    void Start()
    {
        countdown = _delay;
    }


    void Update()
    {
        countdown -= Time.deltaTime;
        if( countdown <= 0.0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Debug.Log("BOOOOOOM!!");
        // Show effect

        // Explode nearby objects
        objectsInExplosion = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D nearbyObjects in objectsInExplosion)
        {
            Rigidbody2D rb = nearbyObjects.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                // Add force
                Vector2 distanceVector = nearbyObjects.transform.position - transform.position;
                if(distanceVector.magnitude > 0)
                {
                    float explosiveForce = _explosionForce / distanceVector.magnitude;
                    rb.AddForce(distanceVector.normalized * _explosionForce);
                }

                //Damage

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
