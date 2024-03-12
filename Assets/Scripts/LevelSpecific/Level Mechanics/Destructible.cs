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
                health -= 10;
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
