using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;

    public float _health;
    public bool _isDead = false;
    private Transform _spawnPoint;

    private void Start()
    {
        _health = _maxHealth;
        _spawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
    }

    private void Update()
    {
        if (_health <= 0)
        {
            KillPlayer();
        }
    }

    public void AddDamage(float damage)
    {
        _health -= damage;
    }

    public bool IsDead() { return _isDead; }

    public void KillPlayer()
    {
        _health = _maxHealth;
        _isDead = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = new Vector3(0, 10, 0);
    }
}
