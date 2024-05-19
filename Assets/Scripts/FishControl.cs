using UnityEngine;

public class FishControl : MonoBehaviour
{
    // Settings that are changeable for each fish
    public float minMoveSpeed = 0.5f;
    public float maxMoveSpeed = 4.0f;
    public float turnSpeed = 150f;

    private float currentSpeed;

    // Map bounds
    private Vector2 minBounds = new Vector2(-16f, -9f);
    private Vector2 maxBounds = new Vector2(16f, 9f);

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(float rotationInput, float speedInput)
    {
        float rotation = Mathf.Clamp(rotationInput, -1f, 1f);
        float targetSpeed = Mathf.Clamp(speedInput, minMoveSpeed, maxMoveSpeed);

        // Smoothly interpolate the speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 2f);

        // Apply smooth rotation
        transform.Rotate(Vector3.forward, rotation * turnSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.right * currentSpeed * Time.deltaTime;

        // Clamp position within bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            transform.position.z);

        // Adjust sprite orientation to always appear upright
        AdjustSpriteOrientation();
    }

    // Adjust the rotation of the sprite so the fish always looks upright
    private void AdjustSpriteOrientation()
    {
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 90f && zRotation < 270f)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    // Check if the fish is swimming against the border of the map
    public bool IsOutOfBounds()
    {
        return transform.position.x >= maxBounds.x || transform.position.x <= minBounds.x ||
               transform.position.y >= maxBounds.y || transform.position.y <= minBounds.y;
    }
}
