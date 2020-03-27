using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform leftDest, rightDest, leftOrig, rightOrig;
    public GameObject left, right;
    private bool open = false;

    void Start()
    {
        leftOrig = left.transform;
        rightOrig = right.transform;
    }

    void Update()
    {
        if (open)
        {
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
        }
        //else
        //{
        //    left.transform.position = Vector3.Lerp(leftOrig.position, left.transform.position, 0.01f);
        //    right.transform.position = Vector3.Lerp(rightOrig.position, right.transform.position, 0.01f);
        //}
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            print("Enter");
            open = true;
        }
    }

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        print("Exit");
    //        open = false;
    //    }
    //}
}
