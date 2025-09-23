using UnityEngine;
using UnityEngine.AI;

public class FinalBossScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
   
    public Transform player;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float stoppingDistance = 3.0f;
    private float nextAttackTime = 0f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashDelay = 1f;
    public float detectionAngle = 90f;
    public float detectionDistance = 10f;
    private bool isWaiting = false;
    private bool isDashing = false;
    private bool dashReady = true;
    private Vector3 dashDirection;

    // Public function to return a random float between min and max
    public float GetRandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (player == null)
            return;

       // if (isDashing) return;
      


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
            if (Time.time >= nextAttackTime)
            {
                Attack();
                // Schedule next attack between 2 and 4 seconds from now
                nextAttackTime = Time.time + GetRandomFloat(2f, 4f);
            }
           
        }
    }

    private void Attack()
    {

        if (isDashing) return;
        Vector3 lookPosition = player.position;
        lookPosition.y = transform.position.y;
        Debug.Log("Final Boss Attack!");
        // Example usage: attack only if random float is greater than 2
        if (GetRandomFloat(0f, 3f) > 1f && GetRandomFloat(0f, 3f) < 2)
        {
            if (PlayerInFront() && dashReady)
            {
                StartCoroutine(PrepareAndDash());
            }
            // Attack logic here
            Debug.Log("Random float triggered attack!");

        }
        if (GetRandomFloat(0f, 3f) > 2f && GetRandomFloat(0f, 3f) < 3)
        {

        }
        if (GetRandomFloat(0f, 3f) > 0f && GetRandomFloat(0f, 3f) < 1) 
        { 
       
        }
    }
    System.Collections.IEnumerator PrepareAndDash()
    {
        dashReady = false;
        agent.enabled = false; // stop navmesh movement
        rb.linearVelocity = Vector3.zero;

        // Wait before dashing
        yield return new WaitForSeconds(dashDelay);

        // Compute dash direction (XZ only)
        Vector3 targetXZ = new Vector3(player.position.x, transform.position.y, player.position.z);
        dashDirection = (targetXZ - transform.position).normalized;

        isDashing = true;
        rb.AddForce(dashDirection * dashSpeed, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector3.zero;
        isDashing = false;
        agent.enabled = true;

        // Force patrol to resume cleanly
        


        // Cooldown before another dash can happen
        yield return new WaitForSeconds(2f); // cooldown
        dashReady = true;
    }
    bool PlayerInFront()
    {
        if (player == null) return false;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float angle = Vector3.Angle(transform.forward, toPlayer);
        float distance = toPlayer.magnitude;

        return angle < detectionAngle * 0.5f && distance < detectionDistance;
    }
    private void OnCollisionStay(Collision collision)
    {
        print("Colliding with: " + collision.transform.name);

        if (collision.transform.CompareTag("Player"))
        {

        }

    }
}

