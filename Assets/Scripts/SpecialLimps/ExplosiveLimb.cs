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
        countdown = _timer;
        _specialLimb = true;
    }

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
