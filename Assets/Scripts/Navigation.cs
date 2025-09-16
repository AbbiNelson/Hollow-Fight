using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent enemy;
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.destination = player.position;
    }
}