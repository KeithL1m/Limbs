using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructible : MonoBehaviour
{
    public float health = 50;
    public Image[] wallStates;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Limb")
        {
            Debug.Log(health);
            health -= 10;

            if (health <= 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
