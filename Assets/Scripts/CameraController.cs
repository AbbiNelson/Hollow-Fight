using UnityEngine;
using System.Collections;
using System;

public class CameraControllor : MonoBehaviour
{
    [Range(0, 1)]
    public float smoothTime;

    public Transform playerTransform;
    public float amountToZoom = 10f;
    private float zoom = 60f;

    [HideInInspector]

    public void Awake()
    {
        zoom = GetComponent<Camera>().fieldOfView;
    }

    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        //pos.x = Mathf.Clamp(pos.x, 0 + (orthoSize * 1.775f), worldSize - (orthoSize * 1.775f));

        GetComponent<Transform>().position = pos;
    }

    public void JumpZoom()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomInAndOut());
    }

    private IEnumerator ZoomInAndOut()
    {
        Camera cam = GetComponent<Camera>();
        cam.fieldOfView = zoom + (amountToZoom * Time.deltaTime);
        yield return new WaitForSeconds(0.25f);
        cam.fieldOfView = zoom;
    }
}