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
    private float nextShootTime;
    public int projectileSpeed;

    // Boss health
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private Animator bossAnimator;
    private Transform player;

    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalY = transform.position.y;
        currentHealth = maxHealth;
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
        if (Time.time >= nextShootTime)
        {
            ShootPearl();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void ShootPearl()
    {
        Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;
        GameObject newPearl = Instantiate(pearlPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D pearlRigidbody = newPearl.GetComponent<Rigidbody2D>();
        pearlRigidbody.velocity = directionToPlayer * projectileSpeed;
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
        Destroy(gameObject);
    }
}