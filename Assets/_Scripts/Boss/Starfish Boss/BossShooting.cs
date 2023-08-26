using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public string poolTag; // The tag associated with the projectiles in the object pooler
    public Transform shootPoint;
    public int numberOfProjectiles = 5;
    public float spreadAngle = 45.0f;
    public float shootInterval = 2.0f;
    public float projectileSpeed = 5.0f;

    private float nextShootTime;
    private ObjectPool objectPool;

    private void Start()
    {
        nextShootTime = Time.time + shootInterval;
        objectPool = ObjectPool.instance; // Reference to the ObjectPool instance
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
            Quaternion rotation = Quaternion.Euler(0, 0, -spreadAngle / 2 + angleIncrement * i);
            Vector2 direction = rotation * Vector2.left;

            GameObject newProjectile = objectPool.GetObjectFromPool();

            if (newProjectile != null)
            {
                newProjectile.transform.position = shootPoint.position;
                newProjectile.transform.rotation = Quaternion.identity;

                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = direction * projectileSpeed;

                // Start the coroutine to return the projectile after 3 seconds
                StartCoroutine(ReturnToPoolAfterDelay(newProjectile));
            }
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject projectile)
    {
        yield return new WaitForSeconds(3.0f); // Wait for 3 seconds
        objectPool.ReturnObjectToPool(projectile); // Return the projectile to the pool
    }
}
