using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LockedDoor : MonoBehaviour
{
    public bool killDoor = false;
    public List<GameObject> killList = new List<GameObject>();
    private bool allDead = false;
    private float numAlive;
    private float numStart;
    public int activated;
    public int requiredActivations;
    public Transform leftDest, rightDest;
    public GameObject left, right;
    public Light doorLight;

    private bool open = false;
    private Rewired.Player player;

    void Start()
    {
        numAlive = killList.Count;
        numStart = killList.Count;
    }
    
    void Update()
    {
        if (open)
        {
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
        }
        if ((!killDoor && activated >= requiredActivations) || allDead)
        {
            doorLight.color = Color.green;
        }
        if (killDoor && !allDead)
        {
            foreach (GameObject o in killList)
            {
                if (!o.activeSelf)
                {
                    numAlive--;
                }
            }
            if (numAlive == 0)
            {
                allDead = true;
            }
            numAlive = numStart;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (!killDoor && activated >= requiredActivations)
        {
            if (col.gameObject.tag == "Player")
            {
                open = true;
            }
        }
        else if(killDoor && allDead)
        {
            if(col.gameObject.tag == "Player")
            {
                open = true;
            }
        }
    }
}
