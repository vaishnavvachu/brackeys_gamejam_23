using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] Slider healthBar;
    private float _currentHealth; 
    private Rigidbody2D _rb2d;
    

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;  // Initialize current health to maximum
        UpdateHealthBar();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime;

        _rb2d.MovePosition(_rb2d.position + movement);

        if (Input.GetKeyDown(KeyCode.J))
        {
            SlashAttack();
        }
    }

    private void SlashAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Handle damaging the enemy here
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UpdateHealthBar();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player's death here
    }

    private void UpdateHealthBar()
    {
        healthBar.value = _currentHealth / maxHealth;  // Update the health bar's value
    }
}
