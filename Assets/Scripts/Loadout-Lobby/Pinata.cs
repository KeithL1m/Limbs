using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    private GameLoader _loader = null;
    private GameManager _gm = null;

    [SerializeField] private List<AudioClip> _clips = null;
    [SerializeField] private AudioClip _pinataDestroy;

    [SerializeField]
    private float _maxHealth;
    private float _health;

    private bool _hit = false;


    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        _gm = ServiceLocator.Get<GameManager>();
        _health = _maxHealth;
    }

    //Dealing damage to Pinata
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Limb")
            return;
        if(collision.gameObject.GetComponent<Limb>().State != Limb.LimbState.Throwing)
            return;
        if (_hit)
            return;
        ServiceLocator.Get<AudioManager>().PlaySound(_pinataDestroy, transform.position, SoundType.SFX);
        ServiceLocator.Get<ParticleManager>().PlayConfettiParticle(transform.position);
        Debug.Log("Pinata is hit");
        _health -= 10.0f;

        if (_health <= 0.0f)
        {
            PinataDestroyed();
        }
    }

    private void PinataDestroyed()
    {
        ServiceLocator.Get<AudioManager>().GameMusic(_clips.ToArray());
        _gm.StartGame();
    }
}
