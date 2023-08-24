using UnityEngine;

public class BossVerticalMovement : MonoBehaviour
{
    public float amplitude = 2.0f; // Amplitude of the up-down motion
    public float frequency = 1.0f; // Frequency of the up-down motion

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Calculate the new position using sine wave
        float newY = startPos.y + amplitude * Mathf.Sin(frequency * Time.time);

        // Update the boss's position only along the Y-axis
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
