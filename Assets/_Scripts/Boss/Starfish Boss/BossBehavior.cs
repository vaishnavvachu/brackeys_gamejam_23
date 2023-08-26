using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    //Attack and Movemnt
    public Transform player;
    public Transform initialPosition;
    public float detectionRange = 10.0f; // The range at which the boss detects the player
    public float moveSpeed = 3.0f;

    private Animator bossAnimator;
    private bool isPlayerNear;
    private bool isMoving;
    private float movementStartTime;
    private bool isMovedFromOriginal = false;
    private bool wasMoving = false;

    //Bubble Shoot
    public string poolTag; // The tag associated with the projectiles in the object pooler
    public Transform shootPoint;
    public int numberOfProjectiles = 5;
    public float spreadAngle = 45.0f;
    public float shootInterval = 2.0f;
    public float projectileSpeed = 5.0f;

    private float nextShootTime;
    private ObjectPool objectPool;
    private bool isShootingEnabled = true;

    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
        nextShootTime = Time.time + shootInterval;
        objectPool = ObjectPool.instance;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootInterval;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isPlayerNear = distanceToPlayer <= detectionRange;

        if (isPlayerNear)
        {
            isMovedFromOriginal = true;
            StopShooting();
            MoveToPlayer();
        }
        else
        {
            if (wasMoving) // Check if the boss was moving before
            {
                StopMoving();
                wasMoving = false; // Reset the flag
            }

            ResumeShooting();
        }

        if (isMoving && HasMovedFor5Seconds())
        {
            StopMoving();
            wasMoving = false; // Reset the flag
        }
    }


    private void MoveToPlayer()
    {
        Vector3 moveDirection = player.position - transform.position;
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime);

        // Set animation parameters or trigger animation here
        bossAnimator.SetBool("IsMoving", true);
        wasMoving = true;
    }

    private void StopMoving()
    {
        isMoving = false;
        // Set animation parameters or trigger animation for idle state here
        bossAnimator.SetBool("IsMoving", false);
        if (isMovedFromOriginal)
        {
            ReturnToInitialPosition();
        }
    }

    private bool HasMovedFor5Seconds()
    {
        return Time.time - movementStartTime >= 5.0f; // Check if 5 seconds have passed
    }

    public void ReturnToInitialPosition()
    {
        StartCoroutine(MoveToInitialPositionSmoothly());
    }

    private void Shoot()
    {
        if (!isShootingEnabled)
            return;
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

    private IEnumerator MoveToInitialPositionSmoothly()
    {
        isMovedFromOriginal = false;
        Vector3 initialPos = initialPosition.position;
        Vector3 startPos = transform.position;
        float journeyLength = Vector3.Distance(startPos, initialPos);
        float startTime = Time.time;

        while (transform.position != initialPos)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, initialPos, fractionOfJourney);
            yield return null;
        }

        // Ensure that the boss arrives exactly at the initial position
        transform.position = initialPos;
        StopShooting();
        ResumeShooting();
    }

    private void StopShooting()
    {
        isShootingEnabled = false;
    }

    private void ResumeShooting()
    {
        isShootingEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Perform the action on the player, such as dealing damage
            DealDamageToPlayer();
        }
    }

    private void DealDamageToPlayer()
    {
        // Implement logic to deal damage to the player here
        Debug.Log("Boss dealt damage to player.");
    }

    private void TakeDamage(int damage)
    {
        bossAnimator.SetTrigger("Hit");
        currentHealth -= damage;
        Debug.Log("Boss took " + damage + " damage. Current health: " + currentHealth);

        if(currentHealth <= 50)
        {
            numberOfProjectiles = 8;
        }

        if (currentHealth <= 0)
        {
            // Boss defeated logic
            Defeated();
        }
    }

    private void Defeated()
    {
        // Implement boss defeated logic here
        Debug.Log("Boss defeated!");
    }
}