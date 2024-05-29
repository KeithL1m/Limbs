using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    GameLoader _loader = null;
    GameManager _gm = null;
    AudioManager _audioManager;

    [SerializeField]
    public float _maxHealth;
    public float _health;
    [SerializeField]
    private Chain chain;
    public DeathPosition[] deathPositions;

    public bool isDead = false;

    [SerializeField]
    private Slider healthSlider;
    private HealthBar _healthBar;

    [SerializeField] private DamageParticles damageParticles;

    [SerializeField] private Material _grayMaterial;
    [SerializeField] private Material _standardMaterial;
    [SerializeField] private Material _lowHealthMaterial;

    [SerializeField] private List<AudioClip> _hurtEffects;
    [SerializeField] private AudioClip _deathSound;


    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _health = _maxHealth;
    }

    public void AddDamage(float damage)
    {
        if (_gm.startScreen)
        {
            return;
        }
        else if (isDead)
            return;

        _audioManager.PlayRandomSound(_hurtEffects.ToArray(), transform.position, SoundType.SFX);

        _health -= damage;
        damageParticles.PlayDamageParticle();

        if (_health <= 0)
        {
            KillPlayer();
        }

        UpdateHealthSlider();
    }

    public bool IsDead() { return isDead; }

    public void KillPlayer()
    {
        if (isDead)
        {
            return;
        }
        _audioManager.PlaySound(_deathSound, transform.position, SoundType.SFX, 0.6f);
        isDead = true;
        _healthBar.SetMaterial(_grayMaterial);
        ServiceLocator.Get<ParticleManager>().PlayDeathParticle(transform.position-transform.up.normalized*0.5f);
        if (_health > 0)
        {
            _health = 0;
            UpdateHealthSlider();
        }

        isDead = true;
        if (!_gm.IsGameOver)
        {
            GetComponent<Player>().Death();
        }
        StartCoroutine(WaitCreateRespawnParticle());

        if (deathPositions[0].Occupied)
        {
            transform.position = deathPositions[1].transform.position;
            chain.EnableChain(deathPositions[1].transform);
            deathPositions[1].Occupied = true;
        }
        else if (deathPositions[1].Occupied)
        {
            transform.position = deathPositions[2].transform.position;
            chain.EnableChain(deathPositions[2].transform);
            deathPositions[2].Occupied = true;
        }
        else
        {
            transform.position = deathPositions[0].transform.position;
            chain.EnableChain(deathPositions[0].transform);
            deathPositions[0].Occupied = true;
        }
        _gm.CheckGameOver();
    }

   

    public void Drop(Vector2 pos) 
    {
        ServiceLocator.Get<ParticleManager>().PlayRespawnParticle(pos);
    }

    IEnumerator WaitCreateRespawnParticle() 
    {
        yield return new WaitForSeconds(0.5f);
        ServiceLocator.Get<ParticleManager>().PlayRespawnParticle(transform.position);
    }

    public void ResetHealth()
    {
        deathPositions = FindObjectsOfType<DeathPosition>();
        _healthBar.SetMaterial(_standardMaterial);

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

        if (_health <= _maxHealth / 5 && isDead == false)
        {
            _healthBar.SetMaterial(_lowHealthMaterial);
        }
    }

    // Add this method to set the health slider
    public void SetHealthSlider(Slider slider)
    {
        healthSlider = slider;
    }

    public void SetHealthBar(HealthBar healthbar)
    {
        _healthBar = healthbar;
    }
}
