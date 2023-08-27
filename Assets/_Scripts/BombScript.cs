using System.Collections;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private int damage;
    private float throwForce;

    private ObjectPool pearlObjectPool;

    private void Awake()
    {
        pearlObjectPool = ObjectPool.instance; // Get the object pool reference
    }

    public void InitializeBomb(int bombDamage, float bombThrowForce)
    {
        damage = bombDamage;
        throwForce = bombThrowForce;
    }

    private void Start()
    {
        Rigidbody2D bombRigidbody = GetComponent<Rigidbody2D>();
        Vector2 throwDirection = transform.right;
        bombRigidbody.velocity = throwDirection * throwForce;

        // Additional initialization if needed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if(collision.collider.CompareTag("Enemy"))
        {
            BossBehavior boss = collision.collider.GetComponent<BossBehavior>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }

        pearlObjectPool.ReturnObjectToPool(gameObject);
    }
}
