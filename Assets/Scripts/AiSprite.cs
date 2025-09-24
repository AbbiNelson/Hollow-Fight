using UnityEngine;

public class AiSprite : MonoBehaviour
{
    public GameObject enemyTransform;
    public Vector3 originaltransform;
    public Vector3 newtransform;
    public Transform sprite;
    HealthSystem healthSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        newtransform = enemyTransform.transform.position;

        //if (originaltransform.transform.position.x > newtransform.transform.position.x)
        //{
        //    transform.localScale = new Vector3(1, 1, 1);
        Animate();
        //}
        //if (originaltransform.transform.position.x < newtransform.transform.position.x)
        //{
        //    transform.localScale = new Vector3(-1, 1, 1);
        //}
        if (healthSystem.currentHealth == 0)
        {
            Destroy(this.gameObject);
        }

     originaltransform = enemyTransform.transform.position;
        
        transform.position = enemyTransform.transform.position;
    }
    private void Animate()
    {
        if (newtransform.x > originaltransform.x) //|| input.magnitude < -0.1f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (newtransform.x < originaltransform.x) //|| input.magnitude > 0.1f)
        {
            transform.localRotation = Quaternion.Euler(0,0,0);
        }
    }

}
