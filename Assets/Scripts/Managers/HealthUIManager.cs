using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public static HealthUIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object persists between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
