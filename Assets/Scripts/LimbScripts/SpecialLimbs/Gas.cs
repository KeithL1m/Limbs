using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    private float _gasRadiusRadius = 0.5f;
    private float _tickRate = 1;
    private float _lifeTime = 10;

    private List<GameObject> _playersInCloud = new();

    private ObjectPoolManager _objectPoolManager;

    private void Start()
    {
        StartCoroutine(DamageTick());

        var collider = GetComponent<CircleCollider2D>();
        collider.radius = _gasRadiusRadius;
    }

    private IEnumerator DamageTick()
    {
        while(_lifeTime > 0)
        {
            yield return new WaitForSeconds(_tickRate);
            --_lifeTime;
            DoDamage();
        }

        _objectPoolManager.RecycleObject(gameObject);
    }

    private void DoDamage()
    {
        foreach(var player in _playersInCloud)
        {
            // Get the player script 
            player.GetComponent<PlayerHealth>().AddDamage(5);
            // Apply gas effect
        }
    }

    // when players are in the gas
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            _playersInCloud.Add(collision.gameObject);
        }
    }

    // when players are out of the gas
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playersInCloud.Remove(collision.gameObject);
        }
    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _gasRadiusRadius);
    }
}


