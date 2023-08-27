using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private SpriteAnimator spriteAnimator;

    [SerializeField] private float restartDuration = 2f;
    
    private Rigidbody2D _rb2d;

    private string attackAnimationName = PlayerAnimationNames.Attack.ToString();
    private string idleAnimationName = PlayerAnimationNames.Idle.ToString();
    private string takeDamageAnimationName = PlayerAnimationNames.TakeDamage.ToString();
    private string dieAnimationName = PlayerAnimationNames.Die.ToString();
    
    private bool playerAlive = true;
    private OxygenController oxygenController;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        oxygenController = FindObjectOfType<OxygenController>();
        if (oxygenController)
        {
            oxygenController.OnOxygenDepleted += Die;
        }
    }

    private void Update()
    {
        if (!playerAlive) { return; }
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
            // Handle damaging the enemy here
        }
    }

    public void TakeDamage(float damage)
    {
        spriteAnimator.Play(takeDamageAnimationName, idleAnimationName, false);
        
        if (oxygenController)
        {
            oxygenController.ReduceOxygenAmount(damage / 100);
        }
    }

    private void Die()
    {
        spriteAnimator.Play(dieAnimationName, "", false);
        playerAlive = false;
        StartCoroutine(RestartGame(restartDuration));
    }
    
    private IEnumerator RestartGame(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum PlayerAnimationNames
{
    Idle,
    Attack,
    TakeDamage,
    Die
}
