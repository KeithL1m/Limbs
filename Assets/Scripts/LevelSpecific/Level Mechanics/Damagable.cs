using UnityEngine;

public class Damagable : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float _damageOutput;
    [SerializeField] private float _knockbackForce = 1000f;
    [SerializeField] private float _airKnockbackForce = 1000f;

    [Header("Spike Settings")]
    [SerializeField] private bool applyKnockback;
    [SerializeField] private bool _destroyOnTouch;

    private float _knockbackDir;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(("Player")))
        {
            var collider = collision.collider;

            Vector3 collisionPoint = collider.ClosestPoint(transform.position);
            Vector3 collisionNormal = transform.position - collisionPoint;

            if (applyKnockback == true)
            {
                if (collisionNormal.x > 0)
                {
                    _knockbackDir = -1f;
                }

                else
                {
                    _knockbackDir = 1f;
                }

                collider.attachedRigidbody.velocity = Vector2.zero;
                collider.attachedRigidbody.AddForce(new Vector2(_knockbackDir * _knockbackForce, _airKnockbackForce));
            }

            collision.collider.GetComponent<PlayerHealth>().AddDamage(_damageOutput);
        }

        else if (_destroyOnTouch == true)
        {
            Destroy(gameObject);
        }
    }
}
