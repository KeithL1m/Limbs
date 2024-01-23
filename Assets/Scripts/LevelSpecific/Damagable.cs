using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float damageOutput = 20;

    [Header("Bool Settings")]
    [SerializeField] private bool destroyOnTouch;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D icicleCollider;
    [SerializeField] private Collider2D triggerCollider;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        rb.isKinematic = false;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(("Player")))
        {
            collision.collider.GetComponent<PlayerHealth>().AddDamage(damageOutput);
            Destroy(gameObject);
        }

        else if (destroyOnTouch == true)
        {
            Destroy(gameObject);
        }
    }
}
