using UnityEngine;

public class AiSprite : MonoBehaviour
{
    public GameObject enemyTransform;
    private Transform originaltransform;
    private Transform newtransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        originaltransform.position = enemyTransform.transform.position;
        if (originaltransform.transform.position.x > newtransform.transform.position.x )
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (originaltransform.transform.position.x < newtransform.transform.position.x)
        {
                      transform.localScale = new Vector3(-1, 1, 1);
        }
        newtransform.position = enemyTransform.transform.position;
    }
}
