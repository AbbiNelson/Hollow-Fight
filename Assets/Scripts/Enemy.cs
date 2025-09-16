using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    private float currentHealth;

    void Start()
    {
        currentHealth = enemyData.health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}