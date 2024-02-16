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
        if (collision.collider.CompareTag("Limb"))
        {
            if (collision.collider.GetComponent<Limb>().State == Limb.LimbState.Throwing)
            {
                health -= 10;
                Debug.Log(health);
            }

            CheckDeath();

        }
    }
    
    public void CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
