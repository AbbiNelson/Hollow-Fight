using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;

public class EnemyAi : MonoBehaviour
{


    public NavMeshAgent Agent;
    public Transform Player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public GameObject Projectile;
    public float health;
    //public Animator animator;
    public bool IsRunning;
    public float fullSpeed;
    public float walkSpeed;
   

    //patrol
    public Vector3 walkpoint;
    bool walkPointset;
    public float walkpointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;

    private void Update()
    {
      //  animator.SetBool("IsRunning", IsRunning);
        ////check for sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange)
        {
            StartCoroutine(Patroling());
        }
        if (PlayerInSightRange && !PlayerInAttackRange)
        {
            ChasePlayer();
        }
        if (PlayerInSightRange && PlayerInAttackRange)
        {
            AttackPlayer();
        }
    }
    private void Awake()
    {
       // Player = GameObject.Find("PlayerOBJ").transform;
        Agent = GetComponent<NavMeshAgent>();
        
    }

    private IEnumerator Patroling()
    {
        IsRunning = false;
        if (!walkPointset)
        { SearchWalkPoint(); }
        if (walkPointset)
        {
            Agent.SetDestination(walkpoint);

            Vector3 distanceToWalkPoint = transform.position - walkpoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                yield return new WaitForSeconds(3);
                walkPointset = false;
            }
        }
    }
    private void ChasePlayer()
    {
        IsRunning = true;
        Agent.speed = fullSpeed;
        Agent.SetDestination(Player.position);
    }

    private void AttackPlayer()
    {
        Agent.SetDestination(transform.position);
        Vector3 lookPosition = Player.position;
        lookPosition.y = transform.position.y; // Keep the AI's y-axis unchanged
        transform.LookAt(lookPosition);
        if (!alreadyAttacked)
        {
            Agent.isStopped = true;
            Agent.Move(-transform.forward * 2f); // Knockback: move back but keep facing player

            Rigidbody rb = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //GetComponent<Rigidbody>().AddForce(-transform.forward * 10f, ForceMode.Impulse); //linearVelocity= -transform.forward * 2f;

            alreadyAttacked = true;
            Agent.isStopped = false;
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
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    public void DestroyEnemy()
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
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z );//randomZ
        if (Physics.Raycast(walkpoint, -transform.up, 3f, WhatIsGround))
        {
            Agent.speed = walkSpeed;
            walkPointset = true;

        }

    }
   // private void OnCollisionEnter(Collision collision)
  //  {
      //  SearchWalkPoint();
   // }
}

