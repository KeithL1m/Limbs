using System;
using System.Collections;
using UnityEngine;

public class StickyBomb : Limb
{
    // Start is called before the first frame update
    private enum BombStickState
    {
        None,
        Player,
        Environment
    }
    private BombStickState _state = BombStickState.None;

    // countdown
    [SerializeField] private float _timer = 6.0f;
    float countdown = 0.0f;

    [SerializeField] float _delayTimer = 0.2f;

    [SerializeField] Collider2D _collider;
    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;
    private Player _player;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();


        _specialLimbs = true;
        countdown = _timer;
    }

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_delayTimer);
        _collider.enabled = true;
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;

        Debug.Log($"<color=yellow>Sticky Bomb Collided with {collision.gameObject.name} </color>");
        if (string.CompareOrdinal(collision.gameObject.tag, "Player") == 0)
        {
            _state = BombStickState.Player;
            _player = collision.gameObject.GetComponent<Player>();

            // Stick to the player
            transform.SetParent(collision.transform);
            GetComponent<Rigidbody2D>().simulated= false;
        }
        else
        {
            _state = BombStickState.Environment;
        }


        if (State != LimbState.Throwing)
            return;
        if(gameObject != null)
        {
            StartCoroutine(ExplodeAfterDelay(() =>
            {
                ServiceLocator.Get<LimbManager>().RemoveLimb(this);
                Destroy(gameObject);
            }));
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void LateUpdate()
    {
        switch(_state)
        {
            case BombStickState.None:
                break;
            case BombStickState.Player:
                // stick to the player counter (if wanted)

                
                break;
            case BombStickState.Environment:
                // stick to the environment / wall
                LimbRB.velocity = Vector2.zero;
                LimbRB.isKinematic = true;
                LimbRB.freezeRotation = true;
                
                break;
            default:
                break;
        }
    }


    private IEnumerator ExplodeAfterDelay(Action callback)
    {
        yield return new WaitForSeconds(countdown);
        Explode();
        callback?.Invoke();
    }

    void Explode()
    {
        Debug.Log("STICKY BOMB BOOM!!!");

        explosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (Collider2D item in explosionRadius)
        {
            Rigidbody2D item_rigidbody = item.GetComponent<Rigidbody2D>();

            if (item_rigidbody != null)
            {
                Vector2 distanceVector = item.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {

                    float explosion = _explosionForce / distanceVector.magnitude;
                    item_rigidbody.AddForce(distanceVector.normalized * explosion);

                    if(_collider.enabled == true)
                    {
                        if (item.CompareTag("Player"))
                        {
                            item.GetComponent<PlayerHealth>().AddDamage(25);

                        }
                    }

                }
            }
        }

    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

}
