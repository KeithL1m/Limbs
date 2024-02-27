using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private Vector3 respawnLocation;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.CompareTag(("Player")))
        {
            collide.GetComponent<PlayerHealth>().AddDamage(25f);
            respawnLocation = collide.GetComponent<PlayerHealth>().deathPositions[Random.Range(0, 2)].transform.position;
            player = collide.GetComponent<Player>().gameObject;

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
