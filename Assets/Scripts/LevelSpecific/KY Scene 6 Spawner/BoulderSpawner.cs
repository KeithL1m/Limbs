using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject objectToSpawn2;
    public float timeToSpawn;
    private float currentTimeToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimeToSpawn > 0)
        {
            currentTimeToSpawn -= Time.deltaTime;
        }

        else
        {
            SpawnObject();
            currentTimeToSpawn = timeToSpawn;
        }
    }

    public void SpawnObject()
    {
        if (objectToSpawn2 != null)
        {
        Instantiate(objectToSpawn2, transform.position, transform.rotation);

        }

        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
