using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalRBC;

    public bool destroySelf;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb = collision.collider.GetComponent<Rigidbody2D>();
            originalRBC = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(UnstickPlayer());
            Debug.Log("STUCK");
        }

        else
        {
            Debug.Log("No");
            StartCoroutine(UnstickPlayer());
        }
    }

    private IEnumerator UnstickPlayer()
    {
        float unstickTime = Time.realtimeSinceStartup + 5.0f;

        while (Time.realtimeSinceStartup < unstickTime)
        {
            yield return null;
        }
            Debug.Log("Done sticking!");

        if (destroySelf == true)
        {
            Destroy(gameObject);
        }

        rb.constraints = originalRBC;
    }

}
