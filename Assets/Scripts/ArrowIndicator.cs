using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowIndicator : Manager
{
    private Vector2 inputDirection;


    // Update is called once per frame
    private void Update()
    {
        // Get the current input direction using the Input System.
        inputDirection = GetComponentInParent<PlayerInput>().actions["Move"].ReadValue<Vector2>();

        // Calculate the rotation angle based on the input direction.
        float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

        // Apply the rotation to the arrow indicator.
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
