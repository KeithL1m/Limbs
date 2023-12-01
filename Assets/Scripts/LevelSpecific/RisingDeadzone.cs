using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingDeadzone : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    private bool moveOnStart;
    [SerializeField]
    private bool disableMovement;
    [SerializeField]
    private float damageOutput = 0.10f;
    [SerializeField]
    private float requiredTime = 20;

    private Rigidbody2D rb;

    private float _currentTime;
    private bool targetTimeReached;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (targetTimeReached == false && moveOnStart == true && disableMovement == false) 
        {
            _currentTime += Time.fixedDeltaTime;
        }

        if (_currentTime >= requiredTime)
        {
            isMoving = true;
            MoveDeadZone();
        }
    }

    private void OnTriggerStay2D(Collider2D collide)
    {
        if (collide.CompareTag(("Player")))
        {
            collide.GetComponent<PlayerHealth>().AddDamage(damageOutput);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("StopPosition"))
        {
            isMoving = false;
            targetTimeReached = false;
            requiredTime += 20;
        }
    }

    private void MoveDeadZone()
    {
        if (isMoving == true)
        {
            targetTimeReached = true;
            rb.MovePosition(rb.position + new Vector2(0f, 1.0f) * Time.deltaTime);
        }
    }
}
