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
    private float damageOutput = 1f;
    [SerializeField]
    private float requiredTime = 20;
    [SerializeField]
    private float _tickRate = 0.5f;
    [SerializeField]
    private AudioClip _tickSound;

    private Rigidbody2D rb;

    private float _currentTime;
    private bool targetTimeReached;
    private bool isMoving = false;

    private List<PlayerHealth> _players = new List<PlayerHealth>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(DamageTick());
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

    private IEnumerator DamageTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_tickRate);
            foreach (var player in _players)
            {
                player.AddDamage(damageOutput, true, _tickSound);
            }
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
        else if (collision.CompareTag("Player"))
        {
            _players.Add(collision.GetComponent<PlayerHealth>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _players.Remove(collision.GetComponent<PlayerHealth>());
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
