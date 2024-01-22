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
        _healthPlayer.AddDamage(_damage,transform.position.x);
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction, Vector2 position)
    {
        _direction = new Vector2();
        _direction = direction;
        Debug.Log(direction);
        transform.position = position;
        _rb2d = GetComponent<Rigidbody2D>();
    }
}
