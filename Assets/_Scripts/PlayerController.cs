using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private Slider healthBar;
    [SerializeField] private SpriteAnimator spriteAnimator;
    
    private float _currentHealth; 
    private Rigidbody2D _rb2d;
    
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;  // Initialize current health to maximum
        
        // TODO: Needs a health bar
        // UpdateHealthBar();
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var movement = new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime;

        _rb2d.MovePosition(_rb2d.position + movement);

        if (Input.GetKeyDown(KeyCode.J))
        {
            SlashAttack();
        }
    }

    private void SlashAttack()
    {
        spriteAnimator.Play(PlayerAnimationNames.Attack.ToString().ToLower(), PlayerAnimationNames.Idle.ToString().ToLower(), false);
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            spriteAnimator.Play(PlayerAnimationNames.Attack.ToString().ToLower(), "", false);
            // Handle damaging the enemy here
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        //UpdateHealthBar();

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

public enum PlayerAnimationNames
{
    Idle,
    Attack,
    TakeDamage,
    Die
}
