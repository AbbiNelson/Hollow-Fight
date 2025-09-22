using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform parent;
    public Transform player;
    public int damage = 10;
    public float timeAlive = 0f;


    public void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= 5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        parent = this.transform.parent;
        print(parent.name + " bullet colliding");
        print("Colliding with: " + collision.transform.name);
        if (collision.transform.CompareTag("Player"))
        {
            player = collision.transform;
            player.GetComponent<HealthSystem>().TakeDamage(damage, parent);
            Destroy(this.gameObject);
        }
        else
        {
            player = null;
        }
    }
}
