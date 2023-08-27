using System.Collections;
using UnityEngine;

public class SeashellBoss : MonoBehaviour
{
    // Boss movement
    public float movementRange = 3.0f;
    public float moveSpeed = 2.0f;
    private float originalY;
    private int direction = 1;

    // Pearl shooting
    public GameObject pearlPrefab;
    public Transform shootPoint;
    public float shootInterval = 2.0f;
    public float detectionRange = 5.0f;// Range at which the boss detects the player for shooting
    public int projectileSpeed;
    private float nextShootTime;
    private ObjectPool pearlObjectPool;
    private Transform player;

    // Boss health
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private Animator bossAnimator;

    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalY = transform.position.y;
        currentHealth = maxHealth;
        nextShootTime = Time.time + shootInterval;
        pearlObjectPool = ObjectPool.instance; // Replace "PearlObjectPool" with your actual object pool script name
        nextShootTime = Time.time + shootInterval;
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float newY = originalY + direction * movementRange * Mathf.Sin(Time.time * moveSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void HandleShooting()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextShootTime)
        {
            ShootPearl();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void ShootPearl()
    {
        bossAnimator.SetBool("Fire", true);
        if (pearlObjectPool == null)
        {
            Debug.LogError("Pearl Object Pool is not assigned!");
            return;
        }

        GameObject newPearl = pearlObjectPool.GetObjectFromPool(); // Get a pearl from the object pool

        if (newPearl != null)
        {
            newPearl.transform.position = shootPoint.position;
            Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;
            Rigidbody2D pearlRigidbody = newPearl.GetComponent<Rigidbody2D>();
            pearlRigidbody.velocity = directionToPlayer * projectileSpeed;

            // Start the coroutine to return the pearl to the pool after a delay
            StartCoroutine(ReturnPearlToPoolAfterDelay(newPearl));
        }
    }

    private IEnumerator ReturnPearlToPoolAfterDelay(GameObject pearl)
    {
        yield return new WaitForSeconds(3.0f);
        bossAnimator.SetBool("Fire", false);
        pearlObjectPool.ReturnObjectToPool(pearl); // Return the pearl to the pool
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Defeated();
        }
    }

    private void Defeated()
    {
        // Implement boss defeated logic here
        Debug.Log("Boss defeated!");

        // Allow the player to throw bombs
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.EnableBombThrowing(); // Implement this method to enable the bomb-throwing ability
        }

        Destroy(gameObject);
    }
}
