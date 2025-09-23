using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public GameObject lookingFor;

    private void Update()
    {
        if (lookingFor == null)
        {
            Destroy(gameObject);
        }
    }
}
