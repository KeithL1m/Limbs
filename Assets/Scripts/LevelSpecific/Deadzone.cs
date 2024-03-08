using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private Vector3 respawnLocation;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.CompareTag(("Player")))
        {
            player = collide.GetComponent<Player>().gameObject;
            collide.GetComponent<PlayerHealth>().AddDamage(25f);

            var playerHealth = collide.GetComponent<PlayerHealth>();
            int randIndex = Random.Range(0, 2);
            Debug.Log($"Random Index: {randIndex}");
            var deathPos = playerHealth.deathPositions[randIndex];
            respawnLocation = deathPos.transform.position; 
            player.transform.position = respawnLocation;
        }


        if(collide.gameObject.tag == "Props")
        {
            Destroy(collide.gameObject);
        }
        else if (collide.gameObject.tag == "Limb")
        {
            ServiceLocator.Get<LimbManager>().RemoveLimb(collide.GetComponent<Limb>());
            Destroy(collide.gameObject);
        }
    }
}
