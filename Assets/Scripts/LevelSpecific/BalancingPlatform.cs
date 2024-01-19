using UnityEngine;

public class BalancingPlatform : MonoBehaviour
{
    public float maxRotation = 30f;

    private Rigidbody2D platformRigidbody;
    private Quaternion initialRotation;

    void Start()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        // Get the current rotation angle
        float currentRotation = transform.rotation.eulerAngles.z;

        // Clamp the rotation angle within the specified range
        float clampedRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        // Apply the clamped rotation
        transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, clampedRotation);
    }
}
