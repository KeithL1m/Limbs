using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    // Spinning sawblade
    [SerializeField]
    private float _rotateSpeed;

    // For moving saw blades
    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private Transform pos1;
    [SerializeField]
    private Transform pos2;
    bool turnback;

    // Additional boost if the chainsaw stop moving
    [SerializeField]
    private Rigidbody2D _sawChain;

    void Start()
    {
        
    }

    void Update()
    {
        if (_isMoving == true)
        {
            MovingSaw();
        }

        transform.Rotate(0.0f, 0.0f, _rotateSpeed * Time.deltaTime);

        // Check if chainsaw slows down
        if(_sawChain.GetComponent<Rigidbody2D>().velocity.magnitude < 10.0f)
        {
            _sawChain.AddForce(new Vector2(100.0f, 0.0f));
        }
    }

    void MovingSaw()
    {
        if (transform.position.x >= pos1.position.x)
            turnback= true;
        if (transform.position.x <= pos2.position.x)
            turnback = false;
        if(turnback)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos2.position, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pos1.position, _rotateSpeed * Time.deltaTime);
        }

    }
}
