using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    [Header("Bool Settings")]
    [SerializeField]
    private bool activateOnStart;
    [SerializeField]
    private bool activateOnTouch;
    
    [Header("Scale Settings")] 
    [SerializeField]
    private float scaleCheck;
    [SerializeField]
    private float tolerenceCheck = 0.1f;
    [SerializeField]
    private float shrinkSpeed = 5f;
    [SerializeField]
    private float shrinkValue = 5f;

    private bool isShrinking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (activateOnStart == true)
        {
            isShrinking = true;
        }
    }

// Update is called once per frame
    void Update()
    {
        if (isShrinking == true)
        {
            gameObject.transform.localScale = (Vector3.Lerp(transform.localScale, transform.localScale / shrinkValue, Time.deltaTime / shrinkSpeed));

            if (Mathf.Abs(gameObject.transform.localScale.x - scaleCheck) < tolerenceCheck)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activateOnTouch == true)
        {
            isShrinking = true;
        }
    }
}
