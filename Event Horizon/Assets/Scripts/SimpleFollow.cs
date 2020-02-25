using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public GameObject Player;
    private Vector3 offset;
    public float moveSpeed = 0.05f;
    public bool useOffset = true;
    private float stunTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (useOffset)
        {
            offset = transform.position - Player.transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > stunTimer)
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, moveSpeed);
        }
        
    }

    private void stun()
    {
        stunTimer = Time.time + 3f;
        print("Stunned");
    }
}
