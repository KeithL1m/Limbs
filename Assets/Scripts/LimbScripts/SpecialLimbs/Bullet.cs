using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private Vector2 _direction;
    private Rigidbody2D _rb2d;
    protected float screenShakePower = 1.2f;
    [SerializeField] protected float screenShakePercent = 1;
    [SerializeField] protected float screenShakeTime = 0.5f;
    private void Update()
    {
        if (_direction == null)
        {
            return;
        }

        _rb2d.velocity = new Vector3(_speed * _direction.x, _speed * _direction.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Destructible")
        {
            collision.gameObject.GetComponent<Destructible>().DamageWall(10);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            return;
        }
        if (ServiceLocator.Get<CameraManager>() != null)
            ServiceLocator.Get<CameraManager>().StartScreenShake(screenShakePower * screenShakePercent, screenShakeTime);

        PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
        _healthPlayer.AddDamage(_damage);
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction, Vector2 position, Sprite _sprite)
    {
        _direction = new Vector2();
        _direction = direction;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        transform.position = position;
        _rb2d = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}
