using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;

    public float _health;
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

    public bool IsDead() { return _health < 0; }

    public void KillPlayer()
    {
        _health = _maxHealth;
        transform.position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, 0f);
    }
}
