using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    [SerializeField]
    private float scaleCheck;
    [SerializeField]
    private float tolerenceCheck = 0.1f;
    [SerializeField]
    private float shrinkSpeed = 5f;
    [SerializeField]
    private float shrinkValue = 5f;

    private bool isShrinking;
    // Start is called before the first frame update
    void Start()
    {
        isShrinking = true;
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


}
