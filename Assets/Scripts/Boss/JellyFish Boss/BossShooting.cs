using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // The prefab of the projectile to be shot
    public Transform shootPoint; // The position where the projectiles should be spawned
    public int numberOfProjectiles = 5; // Number of projectiles to shoot
    public float spreadAngle = 45.0f; // Angle between projectiles in degrees
    public float shootInterval = 2.0f; // Time interval between shots
    public float projectileSpeed = 5.0f; // Speed of the projectiles

    private float nextShootTime;

    private void Start()
    {
        nextShootTime = Time.time + shootInterval;
    }

    private void Update()
    {
        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void Shoot()
    {
        float angleIncrement = spreadAngle / (numberOfProjectiles - 1);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the direction of the projectile based on the spread angle
            Quaternion rotation = Quaternion.Euler(0, 0, -spreadAngle / 2 + angleIncrement * i);
            Vector2 direction = rotation * Vector2.left;

            // Instantiate the projectile at the shootPoint's position
            GameObject newProjectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            // Apply velocity to the projectile in the specified direction
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * projectileSpeed;
        }
    }
}
