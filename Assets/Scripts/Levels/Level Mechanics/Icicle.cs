using System.Collections;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float damageOutput = 20;
    [SerializeField] private Vector3 originalPos;


    [Header("Bool Settings")]
    [SerializeField] private bool destroyOnTouch;


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D icicleCollider;
    [SerializeField] private Collider2D triggerCollider;

    private void Awake()
    {
        originalPos = gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(("Player")))
        {
            collision.collider.GetComponent<PlayerHealth>().AddDamage(damageOutput);
            gameObject.transform.position = originalPos;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(PauseCollision());
        }

        else if (destroyOnTouch == true)
        {
            gameObject.transform.position = originalPos;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(PauseCollision());
        }
    }

    private IEnumerator PauseCollision()
    {

        triggerCollider.enabled = false;

        yield return new WaitForSecondsRealtime(5);

        triggerCollider.enabled = true;
    }

}
