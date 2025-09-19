using UnityEngine;

public class FinalBossScript : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float stoppingDistance = 3.0f;

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
    }
}

