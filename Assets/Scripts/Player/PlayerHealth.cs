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
    [SerializeField] private GameObject _hitParticles;


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
        if (_gm.startScreen)
            return;

        _health -= damage;
        Instantiate(_hitParticles, transform.position, transform.rotation);
        // Update the health slider value here
        UpdateHealthSlider();
    }

    public void AddDamage(float damage, bool isRight)
    {
        if (_gm.startScreen)
            return;

        _health -= damage;
        GameObject particles = Instantiate(_hitParticles, transform.position, transform.rotation);
        if (!isRight)
            particles.transform.localEulerAngles = new Vector3(0,180,0);

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
        else
        {
            transform.position = deathPositions[0].transform.position;
            chain.EnableChain(deathPositions[0].transform);
            deathPositions[0].Occupied = true;
        }

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
