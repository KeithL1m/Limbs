using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.CompareTag(("Player")))
        {
            collide.GetComponent<PlayerHealth>().KillPlayer();
        }
    }
}
