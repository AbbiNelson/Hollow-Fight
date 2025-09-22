using UnityEngine;

public class FinalBossScript : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float stoppingDistance = 3.0f;

    // Public function to return a random float between min and max
    public float GetRandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > stoppingDistance)
        {
            // Rotate smoothly towards the player.
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Move towards the player.
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Placeholder for attack behavior.
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Final Boss Attack!");
        // Example usage: attack only if random float is greater than 2
        if (GetRandomFloat(1f, 3f) == 1f)
        {
            // Attack logic here
            Debug.Log("Random float triggered attack!");
        }
    }
}

