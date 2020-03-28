using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<GameObject> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    private Vector3 velocity;
    public float minZoom = 40f;
    public float maxZoom = 80f;
    public float zoomLimiter = 1;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }
        move();
        zoom();
    }

    void zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, getGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void move()
    {
        Vector3 centerPoint = getCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float getGreatestDistance()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].activeSelf)
            {
                bounds.Encapsulate(targets[i].transform.position);
            }
        }
        return Mathf.Max(bounds.size.x, bounds.size.z);
    }

    Vector3 getCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].activeSelf)
            {
                bounds.Encapsulate(targets[i].transform.position);
            }
        }
        return bounds.center;
    }
}
