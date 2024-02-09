using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float damageOutput = 20;

    [Header("Bool Settings")]
    [SerializeField] private bool destroyOnTouch;

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
