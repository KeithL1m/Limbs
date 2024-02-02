using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [Header("Bool Settings")]
    [SerializeField] private bool reverseAfterTime;
    [SerializeField] private bool isClockwise = true;

    [Header("Adjustable Speed")]
    public float speed = 1;
    public float timeTillReverse = 5;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private bool isReversing;

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
        ReverseMovement();
    }

    public void ReverseMovement()
    {
        if (reverseAfterTime == true && isReversing == false)
        {
            StartCoroutine(ReverseCoroutine());
        }
    }

    private IEnumerator ReverseCoroutine()
    {
        isReversing = true;

        yield return new WaitForSecondsRealtime(timeTillReverse);

        Debug.Log("Waiting");
        
        if (isClockwise == true)
        {
            isClockwise = false;
        }

        else
        {
            isClockwise = true;
        }

        isReversing = false;
    }
}