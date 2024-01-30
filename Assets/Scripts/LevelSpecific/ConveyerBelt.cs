using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] private bool switchOn = true;
    [SerializeField] private bool isClockwise = true;
    [SerializeField] private float speed = 1;


    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!rb) return;

        Vector2 position = rb.position;

        if (isClockwise)
        {
            rb.position += Vector2.left * speed * Time.fixedDeltaTime;
            if (transform.localScale.x == 1) transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        else
        {
            rb.position += Vector2.right * speed * Time.fixedDeltaTime;
            if (transform.localScale.x == 1) transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        rb.MovePosition(position);
    }
}