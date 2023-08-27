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

    //Damae System
    [SerializeField] private int pearlDamage;
    [SerializeField] private int slashDamage;
    [SerializeField] private int enemyDamage;

    private ObjectPool pearlObjectPool;

    private float _currentHealth;
    private Rigidbody2D _rb2d;

    private string attackAnimationName = PlayerAnimationNames.Attack.ToString();
    private string idleAnimationName = PlayerAnimationNames.Idle.ToString();
    private string takeDamageAnimationName = PlayerAnimationNames.TakeDamage.ToString();
    private string dieAnimationName = PlayerAnimationNames.Die.ToString();
    
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;  // Initialize current health to maximum
        pearlObjectPool = ObjectPool.instance;

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
        spriteAnimator.Play(attackAnimationName, idleAnimationName, false);
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            Debug.Log("boss damage");
            if (enemy.CompareTag("Boss")) // Assuming you have a tag for the boss
            {
                SeashellBoss boss = enemy.GetComponent<SeashellBoss>();
                if (boss != null)
                {
                    boss.TakeDamage(slashDamage);
                }
            }
            if (enemy.CompareTag("Enemy"))
            {
                EnemyController enemys = enemy.GetComponent<EnemyController>();
                if (enemys != null)
                {
                    enemys.TakeDamage(slashDamage);
                }
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pearl"))
        {
            TakeDamage(pearlDamage);
            GameObject pearl = collision.gameObject;
            pearlObjectPool.ReturnObjectToPool(pearl);
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(enemyDamage);
        }
        if (collision.collider.CompareTag("Bubble"))
        {
            TakeDamage(pearlDamage);
            GameObject pearl = collision.gameObject;
            pearlObjectPool.ReturnObjectToPool(pearl);
        }
    }

}

public enum PlayerAnimationNames
{
    Idle,
    Attack,
    TakeDamage,
    Die
}
