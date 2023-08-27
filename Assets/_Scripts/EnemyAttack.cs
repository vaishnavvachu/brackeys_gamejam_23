using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;                // Reference to the player's Transform
    public float detectionRange = 2.0f;      // Range at which the enemy detects the player
    public float attackRange = 1.0f;         // Range at which the enemy attacks the player
    public float attackCooldown = 2.0f;      // Cooldown between attacks

    public Transform shootPoint;             // Point where the bubble is spawned
    public float bubbleSpeed = 5.0f;         // Speed of the bubble

    private bool isPlayerInRange = false;
    private float nextAttackTime = 0.0f;

    private Animator enemyAnim;

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckPlayerInRange();
        if (isPlayerInRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            ResetAttackAnimation();
        }
    }

    private void CheckPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= detectionRange;
    }

    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            enemyAnim.SetBool("Attack", true);
            ShootBubble();
        }
    }

    private void ShootBubble()
    {
        GameObject bubble = ObjectPool.instance.GetObjectFromPool(); // Get an object from the object pooler
        if (bubble != null)
        {
            bubble.transform.position = shootPoint.position;
            Rigidbody2D bubbleRigidbody = bubble.GetComponent<Rigidbody2D>();
            Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;
            bubbleRigidbody.velocity = directionToPlayer * bubbleSpeed;
        }
    }

    // This method is called from the animation event to reset the attack animation state
    public void ResetAttackAnimation()
    {
        enemyAnim.SetBool("Attack", false);
    }
}