using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool AI = false;
    float knockbackForce = 5f;

    public Vector3 knockbackVelocity;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform enemyTrasnform)
    {
        Vector3 dir = enemyTrasnform.position - transform.position;

        print("Direction: " + dir);
        print("Enemy Transform: " + enemyTrasnform.position);
        print("Player Transform: " + transform.position);

        currentHealth -= damage;
        print($"{gameObject.name} took {damage} damage. HP: {currentHealth}");
        if (currentHealth <= 0)
        {
            print(rb.transform.name + " died.");
            Destroy(gameObject);
            return;
        }
        ApplyKnockback(dir);

        //EnemyFlashOnHit flash = GetComponent<EnemyFlashOnHit>();
        //if (flash != null)
        //{
        //    flash.Flash();
        //}

    }

    public void ApplyKnockback(Vector3 direction)
    {
        if (rb == null) return;
        direction.y = 0; // prevent upward knockback
        rb.linearVelocity = direction.normalized * knockbackForce;

        this.knockbackVelocity = direction.normalized * knockbackForce;
    }

    public void Knockback()
    {
        if (knockbackVelocity != Vector3.zero)
        {
            knockbackVelocity = knockbackVelocity * (10 - Time.deltaTime);
            if (knockbackVelocity.magnitude < 0.1f)
            {
                knockbackVelocity = Vector3.zero;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        while(knockbackVelocity != Vector3.zero)
        {
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, 1 * Time.deltaTime);
            if (knockbackVelocity.magnitude < 0.1f)
            {
                knockbackVelocity = Vector3.zero;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10, transform);
        }
    }
}
