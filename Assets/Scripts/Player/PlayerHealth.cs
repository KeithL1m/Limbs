using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    GameLoader _loader = null;
    GameManager _gm = null;

    [SerializeField]
    public float _maxHealth;
    [SerializeField]
    private Chain chain;
    private DeathPosition[] deathPositions;

    public float _health;
    public bool _isDead = false;
    private bool _initialized = false;

    [SerializeField]
    private Slider healthSlider;


    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();
        _health = _maxHealth;
        _initialized = true;
}

    private void Update()
    {
        if (!_initialized)
            return;
        if (_health <= 0 && !_isDead)
        {
            KillPlayer();
        }
    }

    public void AddDamage(float damage)
    {
        _health -= damage;

        // Update the health slider value here
        UpdateHealthSlider();
    }

    public bool IsDead() { return _isDead; }

    public void KillPlayer()
    {
        deathPositions = FindObjectsOfType<DeathPosition>();
        _isDead = true;
        if (deathPositions[0].Occupied)
        {
            transform.position = deathPositions[1].transform.position;
            chain.EnableChain(deathPositions[1].transform);
        }
        transform.position = deathPositions[0].transform.position;
        chain.EnableChain(deathPositions[0].transform);
        deathPositions[0].Occupied = true;

        _gm.CheckGameOver();
    }

    public void ResetHealth()
    {
        _health = _maxHealth;

        // Update the health slider value when resetting health
        UpdateHealthSlider();

        chain.DisableChain();
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            // Assuming your health value ranges from 0 to _maxHealth
            healthSlider.value = _health / _maxHealth;
        }
    }

    // Add this method to set the health slider
    public void SetHealthSlider(Slider slider)
    {
        healthSlider = slider;
    }
}
