
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float waitTimeBetweenPatrols = 2f; // Time to wait before choosing next point
    private float patrolWaitTimer;
    private bool isWaitingForNextPoint;


    private void Awake()
    {
      //  player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        // If we don't have a walk point AND we're not already waiting for a new one
        if (!walkPointSet && !isWaitingForNextPoint)
        {
           
            // Start waiting before searching for a new point
            isWaitingForNextPoint = true;
            patrolWaitTimer = waitTimeBetweenPatrols;
        }

        // If we're currently in the waiting state
        if (isWaitingForNextPoint)
        {
            patrolWaitTimer -= Time.deltaTime;

            if (patrolWaitTimer <= 0f)
            {
                agent.isStopped = false;
                isWaitingForNextPoint = false;
                SearchWalkPoint(); // this will set walkPointSet = true
            }

            return; // Don't move while waiting
        }

        // If we have a valid walk point, move there
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            // If we reached it, mark it unset (next frame will trigger wait)
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                // Force agent to stop completely
                agent.SetDestination(transform.position);
                agent.isStopped = true;
            }
        }
    }


    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }


    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        
        if (agent.enabled == true)
        {
            agent.SetDestination(transform.position);
        }
        Vector3 lookPosition = player.position;
        lookPosition.y = transform.position.y; // Keep the AI's y-axis unchanged
        transform.LookAt(lookPosition);
        if (!alreadyAttacked)
        {
            StartCoroutine(Knockback(-transform.forward, 05f, 0f)); // Knockback for 0.2 seconds


            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //GetComponent<Rigidbody>().AddForce(-transform.forward * 10f, ForceMode.Impulse); //linearVelocity= -transform.forward * 2f;

            alreadyAttacked = true;
           // agent.isStopped = false;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private IEnumerator Knockback(Vector3 direction, float force, float duration)
    {

        agent.enabled = false; // Disable NavMeshAgent
        Rigidbody rabbit = GetComponent<Rigidbody>();
        rabbit.AddForce(direction * force, ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        rabbit.linearVelocity = Vector3.zero; // Stop movement after knockback
        agent.enabled = true; // Re-enable NavMeshAgent
        print(force);
        print(direction);
    }
}
