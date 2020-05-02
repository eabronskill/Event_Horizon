using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LockedDoor : MonoBehaviour
{
    public bool killDoor = false;
    public List<GameObject> killList = new List<GameObject>();
    public GameObject parent;
    private bool allDead = false;
    private float numAlive;
    private float numStart;
    public int activated;
    public int requiredActivations;
    public Transform leftDest, rightDest;
    public GameObject left, right;
    public Light doorLight;
    public AudioSource sound;

    private bool open = false;
    private Rewired.Player player;
    private bool hasPlayed = false;

    void Start()
    {
        
        if (parent != null)
        {
            foreach (Enemy o in parent.GetComponentsInChildren<Enemy>())
            {
                killList.Add(o.gameObject);
            }
            
            print("Kill List:" + killList.Count);
        }
        numAlive = killList.Count;
        numStart = killList.Count;
    }
    
    void Update()
    {
        print("NumAlive:" + numAlive);
        if (open)
        {
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
            if (!sound.isPlaying && !hasPlayed)
            {
                sound.Play();
                hasPlayed = true;
            }
        }
        if ((!killDoor && activated >= requiredActivations) || allDead)
        {
            doorLight.color = Color.green;
        }
        if (killDoor && !allDead)
        {
            foreach (GameObject o in killList)
            {
                if (o == null) //&& !o.activeSelf)
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
