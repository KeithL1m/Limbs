using UnityEngine;
using UnityEngine.UI;

public class Destructible : MonoBehaviour
{
    public float health = 50;
    public Sprite[] wallStates;

    [SerializeField]
    private int caseCheck1 = 40;
    private int caseCheck2 = 20;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Limb"))
        {
            if (collision.collider.GetComponent<Limb>().State == Limb.LimbState.Throwing)
            {
                DamageWall(10);
                Debug.Log(health);
            }

            else
            {
                Debug.Log("Null");
            }
        }

    }
    
    public void DamageWall(float damage)
    {
        health -= damage;

        switch (health)
        {
            case 40:
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = wallStates[0];
                    break;
                }
            case 20:
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = wallStates[1];
                    break;
                }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
