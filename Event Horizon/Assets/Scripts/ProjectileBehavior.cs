using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 2f;
    private Vector3 movementVec;
    public ParticleSystem gunFlash;
    public ParticleSystem bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        movementVec = transform.forward * speed;
        Invoke("destroyProjectile", 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + movementVec);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Enemy")
        {
            bloodEffect = Instantiate(gunFlash, transform.position, transform.rotation);
            bloodEffect.Play();
        }
        // Destroy(blood)
        Destroy(this.gameObject.GetComponent<Collider>());    
    }

    public void destroyProjectile()
    {
        Destroy(this.gameObject);
    }

}
