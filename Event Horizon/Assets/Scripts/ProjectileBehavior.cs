using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 2f;
    private Vector3 movementVec;
    public ParticleSystem gunFlash;

    // Start is called before the first frame update
    void Start()
    {
        movementVec = transform.forward * speed;
        Invoke("destroyProjectile", 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + movementVec);
    }

    private void OnCollisionEnter(Collision coll)
    {
        gunFlash.Play();
        Destroy(this.gameObject);
    }

    private void destroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
