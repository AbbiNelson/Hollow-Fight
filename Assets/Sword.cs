using UnityEngine;

public class Sword : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerStay(Collider other)
    {
        print("Colliding with: " + other.transform.name);
        if (other.transform.CompareTag("Enemy"))
        {
            playerController.enemyAttacked = other.transform;
        }
        else
        {
            playerController.enemyAttacked = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerController.enemyAttacked = null;
    }
}
