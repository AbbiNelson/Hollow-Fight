using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public interface IDamageable
{
    void TakeDamage(int damage, Transform enemyTransform);
}

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool AI = false;
    public float knockbackForceMulti = 10f;

    public Vector3 knockbackVelocity;

    private Rigidbody rb;

    public GameObject HealthCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HealthCanvas = transform.Find("Canvas").gameObject;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform enemyTrasnform)
    {
        float healthPercent = (float)currentHealth / maxHealth;
        float damageFactor = (float)damage / maxHealth;

        // Knockback gets stronger the lower the health percent, and scales with damage dealt
        float power = knockbackForceMulti * damage * (1f + (1f - healthPercent)) * (0.5f + damageFactor);

        print(power);
        Vector3 dir = (transform.position - enemyTrasnform.position).normalized;

        print("Direction: " + dir);
        print("Enemy Transform: " + enemyTrasnform.position);
        print("Player Transform: " + transform.position);

        currentHealth -= damage;
        print($"{gameObject.name} took {damage} damage. HP: {currentHealth}");
        if (currentHealth <= 0)
        {
            if (this.AI == true)
            {
                print(rb.transform.name + " died.");
                Destroy(gameObject);
                return;
            }
            else
            {
                SceneManager.LoadScene("Abigail - Main Menu");
            }
        }
        ApplyKnockback(dir, power);

        //EnemyFlashOnHit flash = GetComponent<EnemyFlashOnHit>();
        //if (flash != null)
        //{
        //    flash.Flash();
        //}

    }

    public void ApplyKnockback(Vector3 direction, float power)
    {
        if (rb == null) return;
        direction.y = 0; // prevent upward knockback
        rb.linearVelocity = Vector3.zero;

        rb.AddForce(direction * power, ForceMode.Impulse);
        //  rb.linearVelocity = direction.normalized * reducedPower;
        if (AI)
        {
            StartCoroutine(FreezeRigidbodyAfterKnockback());
        }
        


    }

    //public void Knockback()
    //{
    //    if (knockbackVelocity != Vector3.zero)
    //    {
    //        knockbackVelocity = knockbackVelocity * (10 - Time.deltaTime);
    //        if (knockbackVelocity.magnitude < 0.1f)
    //        {
    //            knockbackVelocity = Vector3.zero;
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //while (knockbackVelocity != Vector3.zero)
        //{
        //    knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, 1 * Time.deltaTime);
        //    if (knockbackVelocity.magnitude < 0.1f)
        //    {
        //        knockbackVelocity = Vector3.zero;
        //    }
        //    return;
        //}

        if (knockbackVelocity != Vector3.zero)
        {
            knockbackVelocity = knockbackVelocity * (10 - Time.deltaTime);
            if (knockbackVelocity.magnitude < 0.1f)
            {
                knockbackVelocity = Vector3.zero;
            }
        }

        HealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>().value = currentHealth;
    }
 

    

    private IEnumerator FreezeRigidbodyAfterKnockback()
    {
        yield return new WaitForSeconds(0.25f);

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            yield return null; // freeze for one frame
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
