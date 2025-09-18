using UnityEngine;
using UnityEngine.AI;

public class RunAI : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;

    public float waitTime = 2f;
    private float waitTimer;

    public float patrolSpeed = 3.5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashDelay = 1f;
    public float detectionAngle = 90f;
    public float detectionDistance = 10f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    public Transform player;

    private bool isWaiting = false;
    private bool isDashing = false;
    private bool dashReady = true;

    private Vector3 dashDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //  player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentTarget = pointA;
        agent.speed = patrolSpeed;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        if (isDashing) return;

        if (PlayerInFront() && dashReady)
        {
            StartCoroutine(PrepareAndDash());
        }
        else if (!isDashing)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.enabled || isWaiting) return;

        Vector3 flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(currentTarget.position.x, 0, currentTarget.position.z);

        float distance = Vector3.Distance(flatPosition, flatTarget);

        // If agent is close to target point, start waiting before next patrol move
        if (distance < 1f)
        {
            StartCoroutine(WaitAtPoint());
        }
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
        isWaiting = false;
        SafeSetDestination(currentTarget.position);
        ;


        // Cooldown before another dash can happen
        yield return new WaitForSeconds(2f); // cooldown
        dashReady = true;
    }
    System.Collections.IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        SafeSetDestination(currentTarget.position);
        // stop agent from moving
        yield return new WaitForSeconds(waitTime);

        // Switch patrol target
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        SafeSetDestination(currentTarget.position);


        isWaiting = false;
    }
    bool SafeSetDestination(Vector3 position)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(position);
            return true;
        }
        return false;
    }



}
