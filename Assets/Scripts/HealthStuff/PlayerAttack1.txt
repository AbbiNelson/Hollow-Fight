using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 25;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public AudioSource atksoundi;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left-click
        {
            Attack();
            atksoundi.Play();
        }
    }

    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            IDamageable target = enemy.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                // Apply knockback
                HealthSystem hs = enemy.GetComponent<HealthSystem>();
                if (hs != null)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    hs.ApplyKnockback(dir);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
