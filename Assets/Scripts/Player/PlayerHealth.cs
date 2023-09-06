using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private Chain chain;
    private GameObject[] deathPositions;

    public float _health;
    public bool _isDead = false;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (_health <= 0 && !_isDead)
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
        deathPositions = GameObject.FindGameObjectsWithTag("DeathLocation");
        _isDead = true;
        transform.position = deathPositions[0].transform.position;
        chain.EnableChain(deathPositions[0].transform);
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        chain.DisableChain();
    }
}
