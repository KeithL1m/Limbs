using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLimb : Limb
{
    // countdown
    [SerializeField] private float _timer = 3.0f;
    float countdown = 0.0f;

    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;


    protected override void Start()
    {
        base.Start();

        countdown = _timer;
    }
    

    // timer for explosion
    private IEnumerator ExplodeAfterDelay(Action callback)
    {
        yield return new WaitForSeconds(countdown);
        Explode();
        callback?.Invoke();
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
                    
                    if(item.CompareTag("Player"))
                    {
                        item.GetComponent<PlayerHealth>().AddDamage(35);
                    }
                }
            }
        }

    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;
        StartCoroutine(ExplodeAfterDelay(() =>
        {
            Destroy(gameObject);
        }));
        
    }
}
