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

    private Camera _mainCamera;
    private float _currentHealth;
    private Rigidbody2D _rb2d;
    private Vector3 _initialAttackPointPosition;

    private string attackAnimationName = PlayerAnimationNames.Attack.ToString();
    private string idleAnimationName = PlayerAnimationNames.Idle.ToString();
    private string takeDamageAnimationName = PlayerAnimationNames.TakeDamage.ToString();
    private string dieAnimationName = PlayerAnimationNames.Die.ToString();
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _rb2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;  // Initialize current health to maximum

        _initialAttackPointPosition = attackPoint.position;
        // TODO: Needs a health bar
        // UpdateHealthBar();
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var movement = new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime;

        _rb2d.MovePosition(_rb2d.position + movement);

        // Input
        if (Input.GetKeyDown(KeyCode.J))
        {
            SlashAttack();
        }

        // Mouse Input
        var position = transform.position;
        var orbVector = Input.mousePosition - _mainCamera.WorldToScreenPoint(position);
        var angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
 
        attackPoint.position = position;
        attackPoint.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void SlashAttack()
    {
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            spriteAnimator.Play(attackAnimationName, idleAnimationName, false);
            // Handle damaging the enemy here
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        spriteAnimator.Play(takeDamageAnimationName, idleAnimationName, false);
        //UpdateHealthBar();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        spriteAnimator.Play(dieAnimationName, "", false);
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
