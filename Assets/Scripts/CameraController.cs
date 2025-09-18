using UnityEngine;
using System.Collections;

public class CameraControllor : MonoBehaviour
{
    [Range(0, 1)]
    public float smoothTime;

    public Transform playerTransform;

    [HideInInspector]
    //public int worldSize;
    //private float orthoSize;

    //public void Spawn(Vector3 pos)
    //{
    //    GetComponent<Transform>().position = pos;
    //    orthoSize = GetComponent<Camera>().orthographicSize;
    //}

    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        //pos.x = Mathf.Clamp(pos.x, 0 + (orthoSize * 1.775f), worldSize - (orthoSize * 1.775f));

        GetComponent<Transform>().position = pos;
    }
}