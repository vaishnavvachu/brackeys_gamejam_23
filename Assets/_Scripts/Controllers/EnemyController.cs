using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float moveDistance = 0.5f; // Adjust this value for the desired distance
    [SerializeField] private float moveDuration = 1f;

    private int _currentHealth;

    private void Start()
    {
        MoveEnemy();
        _currentHealth = maxHealth;
    }

    private void MoveEnemy()
    {
        float initialY = transform.position.y;
        float targetY = initialY + moveDistance;

        // Move up
        transform.DOMoveY(targetY, moveDuration / 2)
            .SetEase(Ease.Linear)
            .OnComplete(() => MoveDown(initialY));
    }

    private void MoveDown(float initialY)
    {
        // Move down
        transform.DOMoveY(initialY, moveDuration / 2)
            .SetEase(Ease.Linear)
            .OnComplete(MoveEnemy);
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}