using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float movementRange = 3.0f; // The range the object will move up and down
    public float moveSpeed = 2.0f; // The speed of movement

    private float originalY; // The original Y position of the object
    private int direction = 1; // 1 for moving up, -1 for moving down

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        // Calculate the new Y position
        float newY = originalY + direction * movementRange * Mathf.Sin(Time.time * moveSpeed);

        // Move the object
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
