using UnityEngine;

public class BalancingPlatform : MonoBehaviour
{
    public float maxRotation = 30f;
    public float rotationSpeed = 5f;

    private Rigidbody2D platformRigidbody;
    private float targetRotation = 0f;

    void Start()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Smoothly interpolate towards the target rotation
        float currentRotation = Mathf.LerpAngle(platformRigidbody.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Clamp the rotation within the specified range
        float clampedRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        // Set the target rotation for the next FixedUpdate
        targetRotation = clampedRotation;

        // Apply rotation using MoveRotation
        platformRigidbody.MoveRotation(clampedRotation);
    }
}
