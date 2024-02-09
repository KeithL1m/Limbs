using UnityEngine;

public class TestMovement : MonoBehaviour
{
    private Transform originalTransform;
    private GameObject player;


    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player is on {gameObject.name}");
            player = collider.gameObject;
            originalTransform = player.transform.parent;
            player.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Player is leaving {gameObject.name}");
        if (player)
        {
            player.transform.SetParent(originalTransform);
        }
    }
}
