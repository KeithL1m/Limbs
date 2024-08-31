using UnityEngine;

public class BalancingPlatform : MonoBehaviour
{
    public float maxRotation = 30f;
    public float rotationSpeed = 5f;
    public Sprite defaultSprite;
    public Sprite movingSprite;

    private Rigidbody2D platformRigidbody;
    private SpriteRenderer spriteRenderer;
    private float targetRotation = 0f;
    private bool isMoving = false;
    private float previousRotation;

    void Start()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the default sprite
        spriteRenderer.sprite = defaultSprite;

        // Initialize previousRotation
        previousRotation = platformRigidbody.rotation;
    }

    void FixedUpdate()
    {
        // Calculate the rotation step based on rotationSpeed
        float rotationStep = rotationSpeed * Time.fixedDeltaTime;

        // Smoothly interpolate towards the target rotation
        float currentRotation = Mathf.LerpAngle(platformRigidbody.rotation, targetRotation, rotationStep);

        // Clamp the rotation within the specified range
        float clampedRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        // Set the target rotation for the next FixedUpdate
        targetRotation = clampedRotation;

        // Check if the platform is moving based on rotation change
        isMoving = Mathf.Abs(previousRotation - platformRigidbody.rotation) > 0.01f;

        // Apply rotation using MoveRotation
        platformRigidbody.MoveRotation(clampedRotation);

        // Update the sprite based on the movement status
        UpdateSprite();

        // Update previousRotation
        previousRotation = platformRigidbody.rotation;
    }

    void UpdateSprite()
    {
        if (isMoving)
        {
            // Change sprite to the moving sprite
            spriteRenderer.sprite = movingSprite;
        }
        else
        {
            // Change sprite back to the default sprite
            spriteRenderer.sprite = defaultSprite;
        }
    }
}
