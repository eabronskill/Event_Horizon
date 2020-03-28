using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBounds : MonoBehaviour
{
    private Vector3 screenBounds;
    private float width;
    private float height;

    private Plane mousePlane;
    private Ray cameraRay;
    private float intersectionDistance = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
        screenBounds = Camera.main.WorldToViewportPoint(transform.position);


        // 0 - 1
        screenBounds.x = Mathf.Clamp01(screenBounds.x);
        screenBounds.y = Mathf.Clamp01(screenBounds.y);
        //(0,0)

        cameraRay = Camera.main.ViewportPointToRay(screenBounds);
     
        mousePlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
        if (mousePlane.Raycast(cameraRay, out intersectionDistance))
        {
            Vector3 hitPoint = cameraRay.GetPoint(intersectionDistance);
            transform.position = hitPoint;
            Debug.DrawLine(Camera.main.transform.position,hitPoint);
        }
        
    }
}
