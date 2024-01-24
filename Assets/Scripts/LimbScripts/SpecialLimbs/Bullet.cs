using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private Vector2 _direction;
    private Rigidbody2D _rb2d;

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
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            return;
        }

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