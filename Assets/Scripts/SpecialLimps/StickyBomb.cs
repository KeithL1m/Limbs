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

    Collider2D[] explosionRadius = null;
    private float _explosionForce = 300;
    private float _explosionRadius = 5;
    private Player _player;
    private Collider2D _bombCollider;


    void Start()
    {
        State = LimbState.PickUp;
        LimbRB = GetComponent<Rigidbody2D>();
        LimbRB.SetRotation(0);

        Trail.SetActive(false);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        Size = GetComponent<Collider2D>().bounds.size.y;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
        _rVMultiplier = _limbData._returnVelocityMultiplier;
        countdown = _timer;
        _specialLimb = true;
        _bombCollider = GetComponent<Collider2D>();
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
        }
        else
        {
            _state = BombStickState.Environment;
        }




        if (State != LimbState.Throwing)
            return;

        StartCoroutine(ExplodeAfterDelay(() =>
        {
            Destroy(gameObject);
        }));
    }

    private void LateUpdate()
    {
        switch(_state)
        {
            case BombStickState.None:
                break;
            case BombStickState.Player:
                // update this limbs position to the _player position
                gameObject.transform.position = _player.transform.position;
                break;
            case BombStickState.Environment:
                //LimbRB.velocity = Vector2.zero;
                LimbRB.isKinematic = true;
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
        Debug.Log("BOOOOM");

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

                    if (item.CompareTag("Player"))
                    {
                        item.GetComponent<PlayerHealth>().AddDamage(35);
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
