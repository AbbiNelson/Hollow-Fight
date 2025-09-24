using UnityEngine;

public class sword : MonoBehaviour
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
}
