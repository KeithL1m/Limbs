using System;
using System.Collections;
using UnityEngine;

public class ExplosiveLimb : Limb
{
    [SerializeField] Collider2D _collider;

    //Particle
    private ParticleManager _particleManager;

    // countdown
    [SerializeField] private float _timer = 3.0f;
    float countdown = 0.0f;

    float _delayTimer = 0.1f;

    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _specialLimbs = true;
        _particleManager = ServiceLocator.Get<ParticleManager>();
        countdown = _timer;
    }

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        StartCoroutine(EnableAfterDelay());
    }
    //timer for collider not hitting player
    private IEnumerator EnableAfterDelay()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_delayTimer);
        _collider.enabled = true;
        yield break;
    }

    // timer for explosion
    private IEnumerator ExplodeAfterDelay(Action callback)
    {
        yield return new WaitForSeconds(countdown);
        Explode();
        
        callback?.Invoke();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void Explode()
    {
        ServiceLocator.Get<CameraManager>().StartScreenShake(0.2f, 0.25f);
        Debug.Log("BOOOOM");

        explosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        _particleManager.PlayExplosionParticle(gameObject.transform.position);

        foreach (Collider2D item in explosionRadius)
        {
            Rigidbody2D item_rigidbody = item.GetComponent<Rigidbody2D>();

            if(item_rigidbody != null)
            {
                Vector2 distanceVector = item.transform.position - transform.position;
                if(distanceVector.magnitude > 0)
                {
                    float explosion = _explosionForce / distanceVector.magnitude;
                    item_rigidbody.AddForce(distanceVector.normalized * explosion);
                    
                    if(item.CompareTag("Player"))
                    {
                        item.GetComponent<PlayerHealth>().AddDamage(35);
                    }

                    if (item.CompareTag("Destructible"))
                    {
                        item.GetComponent<Destructible>().health -= 35;
                        item.GetComponent<Destructible>().CheckDeath();
                    }
                }
            }
        }

    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, _explosionRadius); // Draw a wire sphere gizmo at the game object's position with the specified radius
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;
        StartCoroutine(ExplodeAfterDelay(() =>
        {
            if (gameObject!= null)
            {
                ServiceLocator.Get<LimbManager>().RemoveLimb(this);
                Destroy(gameObject);
            }
        }));
        
    }
}
