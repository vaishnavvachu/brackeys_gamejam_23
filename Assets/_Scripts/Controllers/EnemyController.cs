using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  
    
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float moveDuration = 1f;
    private Transform _playerTransform;
    private bool _isMovingRight;
    private bool _isReadyToShoot = true;
    private int _currentHealth;
    private void Start()
    {
        MoveEnemy();
        _currentHealth = maxHealth;
    }
    
    
    private void MoveEnemy()
    {
        transform.DOMoveY(2f, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
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