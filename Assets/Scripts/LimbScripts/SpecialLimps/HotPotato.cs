using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotato : Limb
{
    // counter
    [SerializeField] private int _explodeCounter;

    //explosion force
    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;

    //Hot potato phases
    [SerializeField] Sprite _potato5;
    [SerializeField] Sprite _potato4;
    [SerializeField] Sprite _potato3;
    [SerializeField] Sprite _potato2;
    [SerializeField] Sprite _potato1;


    protected override void Start()
    {
        base.Start();
        
        _explodeCounter = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _explodeCounter--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;

        // Art and what happens each state of hot potato
        switch (_explodeCounter)
        {
            case 5:
                // potato sprite 5
                break;
            case 4:
                // potato sprite 4
                break;
            case 3:
                // potato sprite 3
                break;
            case 2:
                // potato sprite 2
                break;
            case 1:
                // potato sprite 1
                break;
            case 0:
                Explode();
                Destroy(gameObject);
                break;
            default:
                break;
        }

    }

    void Explode()
    {
        Debug.Log("BOOOOM");

        explosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (Collider2D item in explosionRadius)
        {
            Rigidbody2D item_rigidbody = item.GetComponent<Rigidbody2D>();

            if (item_rigidbody != null)
            {
                Vector2 distanceVector = item.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {
                    float explosion = _explosionForce / distanceVector.magnitude;
                    item_rigidbody.AddForce(distanceVector.normalized * explosion);

                    if (item.CompareTag("Player"))
                    {
                        item.GetComponent<PlayerHealth>().AddDamage(1000);
                    }
                }
            }
        }

    }
}
