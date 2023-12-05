using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLimb : MonoBehaviour
{
    // countdown
    [SerializeField] private float _timer = 3.0f;
    float countdown = 0.0f;

    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;


    void Start()
    {
        countdown = _timer;
    }

    void Update()
    {
        bool exploded = false;

        countdown -= Time.deltaTime;
        if(countdown <= 0.0f)
        {
            Explode();
            exploded= true;
        }


        if(exploded)
        {
            Destroy(gameObject);
        }


    }

    void Explode()
    { 
        Debug.Log("BOOOOM");

        explosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (Collider2D item in explosionRadius)
        {
            Rigidbody2D item_rigidbody = item.GetComponent<Rigidbody2D>();

            if(item_rigidbody != null)
            {
                Vector2 distanceVector = item.transform.position - transform.position;
                if(distanceVector.magnitude > 0)
                {
                    float explosion = _explosionForce / distanceVector.magnitude;
                    item_rigidbody.AddForce(distanceVector.normalized * explosion);
                }
            }
        }

    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
