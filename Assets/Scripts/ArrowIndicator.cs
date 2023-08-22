using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowIndicator : MonoBehaviour
{
    public GameObject arrowIndicatorPrefab; // Reference to the arrow indicator prefab.
    private GameObject arrowIndicator; // Reference to the instantiated arrow indicator.
    private Vector2 inputDirection;

    void Start()
    {
        arrowIndicator = Instantiate(arrowIndicatorPrefab, transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
