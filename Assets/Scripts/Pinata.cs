using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;
    private float _health;

    private UIManager _manager;


    void Start()
    {
        _health = _maxHealth;    
        _manager = FindObjectOfType<UIManager>();
    }
    // Update is called once per frame

    //Dealing damage to Pinata
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //if(collision.gameObject.tag == "Limb" && collision.gameObject.GetComponent<Limb>()._limbState == Limb.LimbState.Throwing)
        //{
        //    _health -= 10.0f;
        //}
        if (collision.gameObject.tag != "Limb")
        {
            return;
        }
        else if(collision.gameObject.GetComponent<Limb>()._limbState != Limb.LimbState.Throwing)
        {
            return;
        }
        _health -= 10.0f;

        if (_health <= 0.0f)
        {
            PinataDestroyed();
        }
    }

    void PinataDestroyed()
    {
        _manager.SetUpLeaderBoard();
        _manager.UpdateLeaderBoard();

        GameManager.instance.startScreen = false;
        MapManager.instance.LoadMap();
    }
}
