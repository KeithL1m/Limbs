using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;
    private float _health;


    void Start()
    {
        _health = _maxHealth;    
    }

    //Dealing damage to Pinata
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Limb")
            return;
        if(collision.gameObject.GetComponent<Limb>()._limbState != Limb.LimbState.Throwing)
            return;
        
        _health -= 10.0f;

        if (_health <= 0.0f)
        {
            PinataDestroyed();
        }
    }

    private void PinataDestroyed()
    {
        GameManager.instance.StartGame();
    }
}
