using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalRBC;
    [SerializeField]protected float damage = 5f;
    protected List<PlayerHealth> playerHealths = new List<PlayerHealth>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb = collision.GetComponent<Rigidbody2D>();
            originalRBC = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(UnstickPlayer());
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            if (!playerHealths.Contains(playerHealth))
            {
                playerHealths.Add(playerHealth);
            }
            Debug.Log("STUCK");
        }

        else
        {
            Debug.Log("No");
            StartCoroutine(UnstickPlayer());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Player"))
        //{
        //    rb = collision.collider.GetComponent<Rigidbody2D>();
        //    originalRBC = rb.constraints;
        //    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        //    StartCoroutine(UnstickPlayer());
        //    PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
        //    if (!playerHealths.Contains(playerHealth))
        //    {
        //        playerHealths.Add(playerHealth);
        //    }
        //    Debug.Log("STUCK");
        //}

        //else
        //{
        //    Debug.Log("No");
        //    StartCoroutine(UnstickPlayer());
        //}
    }

    private void FixedUpdate()
    {
        if (playerHealths.Count != 0)
        {
            foreach (var item in playerHealths)
            {
                if (item != null)
                    item.AddDamage(damage * Time.fixedDeltaTime);
            }
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            if (!playerHealths.Contains(playerHealth))
            {
                playerHealths.Remove(playerHealth);
            }
            Debug.Log("STUCK");
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
        Destroy(gameObject);
        rb.constraints = originalRBC;
    }

}
