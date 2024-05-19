using UnityEngine;

public class FishControl : MonoBehaviour
{
    public float minMoveSpeed = 1.0f;
    public float maxMoveSpeed = 4.0f;
    private float moveSpeed;
    public float turnSpeed = 150f;

    private Vector2 minBounds = new Vector2(-16f, -9f);
    private Vector2 maxBounds = new Vector2(16f, 9f);

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(float rotationInput, float speedInput)
    {
        Debug.Log($"FishControl - Move: rotationInput={rotationInput}, speedInput={speedInput}");

        float rotation = Mathf.Clamp(rotationInput, -1f, 1f);
        moveSpeed = Mathf.Clamp(speedInput, minMoveSpeed, maxMoveSpeed);

        // Apply rotation
        transform.Rotate(Vector3.forward, rotation * turnSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.right * moveSpeed * Time.deltaTime;

        // Clamp position within bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            transform.position.z);

        // Adjust sprite orientation to always appear upright
        AdjustSpriteOrientation();
    }

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

    public bool IsOutOfBounds()
    {
        return transform.position.x > maxBounds.x || transform.position.x < minBounds.x ||
               transform.position.y > maxBounds.y || transform.position.y < minBounds.y;
    }
}
