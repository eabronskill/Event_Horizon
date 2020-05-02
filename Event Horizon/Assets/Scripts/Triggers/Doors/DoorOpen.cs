using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class DoorOpen : MonoBehaviour
{
    public Transform leftDest, rightDest;
    public GameObject left, right;
    private bool open = false;
    private Rewired.Player player;
    public AudioSource sound;
    private bool hasPlayed = false;

    void Update()
    {
        if (open)
        {
            if (!sound.isPlaying && !hasPlayed)
            {
                sound.Play();
                hasPlayed = true;
            }
            
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            open = true;
        }
    }

}
